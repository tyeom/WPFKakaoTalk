using Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Common.MarkupExtension;

public class ChatMessageBoxSelector : DependencyObject
{
    private static readonly Dictionary<ChattingMessageListControl, ChatMessageBoxSelector> chattingMessageListControls =
        new Dictionary<ChattingMessageListControl, ChatMessageBoxSelector>();

    private readonly ChattingMessageListControl _chattingMessageList;
    private ScrollContentPresenter _scrollContent;

    private SelectionAdorner _selectionRect;
    private AutoScroller _autoScroller;
    private ItemsControlSelector _selector;

    private bool _mouseCaptured;
    private Point _start;
    private Point _end;

    private ChatMessageBoxSelector(ChattingMessageListControl chattingMessageList)
    {
        _chattingMessageList = chattingMessageList;
        if (_chattingMessageList.IsLoaded)
        {
            this.Register();
        }
        else
        {
            // 채팅 리스트(ListBox)의 자식 컨트롤이 모두 로드 완료된 시점을 캐치하기 위해
            // 채팅 리스트(ListBox) 컨트롤이 Loaded 완료 후 Register() 호출 한다.
            _chattingMessageList.Loaded += ChattingMessageList_Loaded;
        }
    }

    public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached("Enabled",
                typeof(bool),
                typeof(ChatMessageBoxSelector),
                new UIPropertyMetadata(false, IsEnabledChangedCallback));

    public static bool GetEnabled(DependencyObject obj)
    {
        return (bool)obj.GetValue(EnabledProperty);
    }

    public static void SetEnabled(DependencyObject obj, bool value)
    {
        obj.SetValue(EnabledProperty, value);
    }

    /// <summary>
    /// 선택 영역 보이기 여부
    /// </summary>
    public static readonly DependencyProperty? IsSelectionRectVisibleProperty =
        DependencyProperty.RegisterAttached(
            "IsSelectionRectVisible",
            typeof(bool),
            typeof(ChattingMessageControl),
            new UIPropertyMetadata(true));

    public static bool GetIsSelectionRectVisible(DependencyObject obj)
    {
        return (bool)obj.GetValue(IsSelectionRectVisibleProperty);
    }

    public static void SetIsSelectionRectVisible(DependencyObject obj, bool value)
    {
        obj.SetValue(IsSelectionRectVisibleProperty, value);
    }

    private static void IsEnabledChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ChattingMessageListControl chattingMessageList = d as ChattingMessageListControl;
        if (chattingMessageList != null)
        {
            if ((bool)e.NewValue)
            {
                if (chattingMessageList.SelectionMode == SelectionMode.Single)
                {
                    chattingMessageList.SelectionMode = SelectionMode.Extended;
                }

                chattingMessageListControls.Add(chattingMessageList, new ChatMessageBoxSelector(chattingMessageList));
            }
            else
            {
                ChatMessageBoxSelector? selector;
                if (chattingMessageListControls.TryGetValue(chattingMessageList, out selector))
                {
                    chattingMessageListControls.Remove(chattingMessageList);
                    selector.UnRegister();
                }
            }
        }
    }

    private bool Register()
    {
        // 스크롤영역 찾기
        _scrollContent = FindChild<ScrollContentPresenter>(_chattingMessageList);
        if (_scrollContent != null)
        {
            _autoScroller = new AutoScroller(_chattingMessageList);
            _autoScroller.OffsetChanged += this.OnOffsetChanged;

            _selectionRect = new SelectionAdorner(_scrollContent);
            _scrollContent.AdornerLayer.Add(_selectionRect);
            _selector = new ItemsControlSelector(_chattingMessageList);

            // 기본적으로 ListBox/View에서 MouseLeftButtonDown 이벤트를 가로채고 더이상 통보하지 않는다.
            // 그래서 PreviewMouseLeftButtonDown 이벤트에서 먼저 처리 되도록 한다.
            _chattingMessageList.PreviewMouseLeftButtonDown += this.OnPreviewMouseLeftButtonDown;
            _chattingMessageList.MouseLeftButtonUp += this.OnMouseLeftButtonUp;
            _chattingMessageList.MouseMove += this.OnMouseMove;
        }

        return (_scrollContent != null);
    }

    private void UnRegister()
    {
        this.StopSelection();

        _chattingMessageList.PreviewMouseLeftButtonDown -= this.OnPreviewMouseLeftButtonDown;
        _chattingMessageList.MouseLeftButtonUp -= this.OnMouseLeftButtonUp;
        _chattingMessageList.MouseMove -= this.OnMouseMove;

        _autoScroller.UnRegister();
    }

    private bool TryCaptureMouse(MouseButtonEventArgs e)
    {
        Point position = e.GetPosition(_scrollContent);

        // 마우스 포인터가 가르키는 컨트롤 찾기
        UIElement element = _scrollContent.InputHitTest(position) as UIElement;
        if (element != null)
        {
            // MouseLeftButtonDown 이벤트 강제 발생
            var args = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left, e.StylusDevice);
            args.RoutedEvent = Mouse.MouseDownEvent;
            args.Source = e.Source;
            element.RaiseEvent(args);

            // 현재 마우스 캡쳐된 것이 채팅 리스트 컨트롤이 아닌 경우
            if (Mouse.Captured != _chattingMessageList)
            {
                return false;
            }
        }

        // 마우스 포인터가 가르키는 컨트롤 없음
        return _scrollContent.CaptureMouse();
    }

    private void StopSelection()
    {
        _selectionRect.IsEnabled = false;
        _autoScroller.IsEnabled = false;
    }

    private void StartSelection(Point location)
    {
        _chattingMessageList.Focus();

        _start = location;
        _end = location;

        // 키보드 Ctrl, Shift 로 드래그 영역 시작인 경우 영역 선택 다시 시작
        if (((Keyboard.Modifiers & ModifierKeys.Control) == 0) &&
            ((Keyboard.Modifiers & ModifierKeys.Shift) == 0))
        {
            _chattingMessageList.SelectedItems.Clear();
        }

        _selector.Reset();
        this.UpdateSelection();

        _selectionRect.IsEnabled = GetIsSelectionRectVisible(_chattingMessageList);
        _autoScroller.IsEnabled = true;
    }

    private void UpdateSelection()
    {
        // 선택영역 시작지점 Point
        Point start = _autoScroller.TranslatePoint(_start);

        // 선택영역 사각형 그리기
        double x = Math.Min(start.X, _end.X);
        double y = Math.Min(start.Y, _end.Y);
        double width = Math.Abs(_end.X - start.X);
        double height = Math.Abs(_end.Y - start.Y);
        Rect area = new Rect(x, y, width, height);
        _selectionRect.SelectionArea = area;

        // 채팅 메세지 리스트 컨트롤 기준으로 포인트 시작지점과 끝지점 포인트 구하기
        Point topLeft = _scrollContent.TranslatePoint(area.TopLeft, _chattingMessageList);
        Point bottomRight = _scrollContent.TranslatePoint(area.BottomRight, _chattingMessageList);

        // 위에서 구한 포인트 영역만큼 아이템 선택
        _selector.UpdateSelection(new Rect(topLeft, bottomRight));
    }

    /// <summary>
    /// 자식 FrameworkElement 찾기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reference"></param>
    /// <returns></returns>
    private static T FindChild<T>(DependencyObject reference) where T : class
    {
        var queue = new Queue<DependencyObject>();
        queue.Enqueue(reference);
        while (queue.Count > 0)
        {
            DependencyObject child = queue.Dequeue();
            T obj = child as T;
            if (obj != null)
            {
                return obj;
            }

            // 큐에 추가하고 다시 찾음
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(child); i++)
            {
                queue.Enqueue(VisualTreeHelper.GetChild(child, i));
            }
        }
        return null;
    }

    private void ChattingMessageList_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.Register())
        {
            _chattingMessageList.Loaded -= this.ChattingMessageList_Loaded;
        }
    }

    private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        // 마우스 포인트가 스크롤 컨텐츠에 있는지 체크
        Point mouse = e.GetPosition(_scrollContent);
        if ((mouse.X >= 0) && (mouse.X < _scrollContent.ActualWidth) &&
            (mouse.Y >= 0) && (mouse.Y < _scrollContent.ActualHeight))
        {
            _mouseCaptured = this.TryCaptureMouse(e);
            if (_mouseCaptured)
            {
                // Start 드래그 영역선택
                this.StartSelection(mouse);
            }
        }
    }

    private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (_mouseCaptured)
        {
            _mouseCaptured = false;
            _scrollContent.ReleaseMouseCapture();
            this.StopSelection();
        }
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (_mouseCaptured)
        {
            // Get the position relative to the content of the ScrollViewer.
            _end = e.GetPosition(_scrollContent);
            _autoScroller.Update(_end);
            this.UpdateSelection();
        }
    }

    private void OnOffsetChanged(object sender, OffsetChangedEventArgs e)
    {
        _selector.Scroll(e.HorizontalChange, e.VerticalChange);
        this.UpdateSelection();
    }

    /// <summary>
    /// 참고 - https://www.codeproject.com/Articles/209560/ListBox-drag-selection
    /// Automatically scrolls an ItemsControl when the mouse is dragged outside
    /// of the control.
    /// </summary>
    private sealed class AutoScroller
    {
        private readonly DispatcherTimer autoScroll = new DispatcherTimer();
        private readonly ItemsControl itemsControl;
        private readonly ScrollViewer scrollViewer;
        private readonly ScrollContentPresenter scrollContent;
        private bool isEnabled;
        private Point offset;
        private Point mouse;

        /// <summary>
        /// Initializes a new instance of the AutoScroller class.
        /// </summary>
        /// <param name="itemsControl">The ItemsControl that is scrolled.</param>
        /// <exception cref="ArgumentNullException">itemsControl is null.</exception>
        public AutoScroller(ItemsControl itemsControl)
        {
            if (itemsControl == null)
            {
                throw new ArgumentNullException("itemsControl");
            }

            this.itemsControl = itemsControl;
            this.scrollViewer = FindChild<ScrollViewer>(itemsControl);
            this.scrollViewer.ScrollChanged += this.OnScrollChanged;
            this.scrollContent = FindChild<ScrollContentPresenter>(this.scrollViewer);

            this.autoScroll.Tick += delegate { this.PreformScroll(); };
            this.autoScroll.Interval = TimeSpan.FromMilliseconds(GetRepeatRate());
        }

        /// <summary>Occurs when the scroll offset has changed.</summary>
        public event EventHandler<OffsetChangedEventArgs> OffsetChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the auto-scroller is enabled
        /// or not.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }
            set
            {
                if (this.isEnabled != value)
                {
                    this.isEnabled = value;

                    // Reset the auto-scroller and offset.
                    this.autoScroll.IsEnabled = false;
                    this.offset = new Point();
                }
            }
        }

        /// <summary>
        /// Translates the specified point by the current scroll offset.
        /// </summary>
        /// <param name="point">The point to translate.</param>
        /// <returns>A new point offset by the current scroll amount.</returns>
        public Point TranslatePoint(Point point)
        {
            return new Point(point.X - this.offset.X, point.Y - this.offset.Y);
        }

        /// <summary>
        /// Removes all the event handlers registered on the control.
        /// </summary>
        public void UnRegister()
        {
            this.scrollViewer.ScrollChanged -= this.OnScrollChanged;
        }

        /// <summary>
        /// Updates the location of the mouse and automatically scrolls if required.
        /// </summary>
        /// <param name="mouse">
        /// The location of the mouse, relative to the ScrollViewer's content.
        /// </param>
        public void Update(Point mouse)
        {
            this.mouse = mouse;

            // If scrolling isn't enabled then see if it needs to be.
            if (!this.autoScroll.IsEnabled)
            {
                this.PreformScroll();
            }
        }

        // Returns the default repeat rate in milliseconds.
        private static int GetRepeatRate()
        {
            // The RepeatButton uses the SystemParameters.KeyboardSpeed as the
            // default value for the Interval property. KeyboardSpeed returns
            // a value between 0 (400ms) and 31 (33ms).
            const double Ratio = (400.0 - 33.0) / 31.0;
            return 400 - (int)(SystemParameters.KeyboardSpeed * Ratio);
        }

        private double CalculateOffset(int startIndex, int endIndex)
        {
            double sum = 0;
            for (int i = startIndex; i != endIndex; i++)
            {
                FrameworkElement container = this.itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
                if (container != null)
                {
                    // Height = Actual height + margin
                    sum += container.ActualHeight;
                    sum += container.Margin.Top + container.Margin.Bottom;
                }
            }
            return sum;
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // Do we need to update the offset?
            if (this.IsEnabled)
            {
                double horizontal = e.HorizontalChange;
                double vertical = e.VerticalChange;

                // VerticalOffset means two seperate things based on the CanContentScroll
                // property. If this property is true then the offset is the number of
                // items to scroll; false then it's in Device Independant Pixels (DIPs).
                if (this.scrollViewer.CanContentScroll)
                {
                    // We need to either increase the offset or decrease it.
                    if (e.VerticalChange < 0)
                    {
                        int start = (int)e.VerticalOffset;
                        int end = (int)(e.VerticalOffset - e.VerticalChange);
                        vertical = -this.CalculateOffset(start, end);
                    }
                    else
                    {
                        int start = (int)(e.VerticalOffset - e.VerticalChange);
                        int end = (int)e.VerticalOffset;
                        vertical = this.CalculateOffset(start, end);
                    }
                }

                this.offset.X += horizontal;
                this.offset.Y += vertical;

                var callback = this.OffsetChanged;
                if (callback != null)
                {
                    callback(this, new OffsetChangedEventArgs(horizontal, vertical));
                }
            }
        }

        private void PreformScroll()
        {
            bool scrolled = false;

            if (this.mouse.X > this.scrollContent.ActualWidth)
            {
                this.scrollViewer.LineRight();
                scrolled = true;
            }
            else if (this.mouse.X < 0)
            {
                this.scrollViewer.LineLeft();
                scrolled = true;
            }

            if (this.mouse.Y > this.scrollContent.ActualHeight)
            {
                this.scrollViewer.LineDown();
                scrolled = true;
            }
            else if (this.mouse.Y < 0)
            {
                this.scrollViewer.LineUp();
                scrolled = true;
            }

            // It's important to disable scrolling if we're inside the bounds of
            // the control so that when the user does leave the bounds we can
            // re-enable scrolling and it will have the correct initial delay.
            this.autoScroll.IsEnabled = scrolled;
        }
    }

    /// <summary>
    /// 참고 - https://www.codeproject.com/Articles/209560/ListBox-drag-selection
    /// Draws a selection rectangle on an AdornerLayer.
    /// </summary>
    private sealed class SelectionAdorner : Adorner
    {
        private Rect selectionRect;

        /// <summary>
        /// Initializes a new instance of the SelectionAdorner class.
        /// </summary>
        /// <param name="parent">
        /// The UIElement which this instance will overlay.
        /// </param>
        /// <exception cref="ArgumentNullException">parent is null.</exception>
        public SelectionAdorner(UIElement parent)
            : base(parent)
        {
            // Make sure the mouse doesn't see us.
            this.IsHitTestVisible = false;

            // We only draw a rectangle when we're enabled.
            this.IsEnabledChanged += delegate { this.InvalidateVisual(); };
        }

        /// <summary>Gets or sets the area of the selection rectangle.</summary>
        public Rect SelectionArea
        {
            get
            {
                return this.selectionRect;
            }
            set
            {
                this.selectionRect = value;
                this.InvalidateVisual();
            }
        }

        /// <summary>
        /// Participates in rendering operations that are directed by the layout system.
        /// </summary>
        /// <param name="drawingContext">The drawing instructions.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (this.IsEnabled)
            {
                // Make the lines snap to pixels (add half the pen width [0.5])
                double[] x = { SelectionArea.Left + 0.5, SelectionArea.Right + 0.5 };
                double[] y = { SelectionArea.Top + 0.5, SelectionArea.Bottom + 0.5 };
                drawingContext.PushGuidelineSet(new GuidelineSet(x, y));

                Brush fill = SystemColors.HighlightBrush.Clone();
                fill.Opacity = 0.4;
                drawingContext.DrawRectangle(fill, new Pen(SystemColors.HighlightBrush, 1.0), this.SelectionArea);
            }
        }
    }

    /// <summary>
    /// 참고 - https://www.codeproject.com/Articles/209560/ListBox-drag-selection
    /// Enables the selection of items by a specified rectangle.
    /// </summary>
    private sealed class ItemsControlSelector
    {
        private readonly ItemsControl itemsControl;
        private Rect previousArea;

        /// <summary>
        /// Initializes a new instance of the ItemsControlSelector class.
        /// </summary>
        /// <param name="itemsControl">
        /// The control that contains the items to select.
        /// </param>
        /// <exception cref="ArgumentNullException">itemsControl is null.</exception>
        public ItemsControlSelector(ItemsControl itemsControl)
        {
            if (itemsControl == null)
            {
                throw new ArgumentNullException("itemsControl");
            }
            this.itemsControl = itemsControl;
        }

        /// <summary>
        /// Resets the cached information, allowing a new selection to begin.
        /// </summary>
        public void Reset()
        {
            this.previousArea = new Rect();
        }

        /// <summary>
        /// Scrolls the selection area by the specified amount.
        /// </summary>
        /// <param name="x">The horizontal scroll amount.</param>
        /// <param name="y">The vertical scroll amount.</param>
        public void Scroll(double x, double y)
        {
            this.previousArea.Offset(-x, -y);
        }

        /// <summary>
        /// Updates the controls selection based on the specified area.
        /// </summary>
        /// <param name="area">
        /// The selection area, relative to the control passed in the contructor.
        /// </param>
        public void UpdateSelection(Rect area)
        {
            int selectedItemCount = 0;
            ChattingMessageControl? tmpSelectionChattingMessageControl =
                null;

            // Check eack item to see if it intersects with the area.
            for (int i = 0; i < this.itemsControl.Items.Count; i++)
            {
                FrameworkElement item = this.itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
                if (item != null)
                {
                    var chattingMessageControl = FindChild<ChattingMessageControl>(item);
                    chattingMessageControl.IsSelection = false;

                    // Get the bounds in the parent's co-ordinates.
                    Point topLeft = item.TranslatePoint(new Point(0, 0), this.itemsControl);
                    Rect itemBounds = new Rect(topLeft.X, topLeft.Y, item.ActualWidth, item.ActualHeight);

                    // Only change the selection if it intersects with the area
                    // (or intersected i.e. we changed the value last time).
                    if (itemBounds.IntersectsWith(area))
                    {
                        Selector.SetIsSelected(item, true);

                        // 선택된 자식이 2개 이상이라면 임시 보관한 자식 컨트롤 선택 효과 설정
                        if(selectedItemCount == 2 && tmpSelectionChattingMessageControl != null)
                        {
                            tmpSelectionChattingMessageControl.IsSelection = true;
                        }
                        // 최초 선택 대상이라면 선택할 자식 컨트롤 임시 보관
                        if (selectedItemCount == 0)
                        {
                            tmpSelectionChattingMessageControl = chattingMessageControl;
                        }
                        else
                        {
                            chattingMessageControl.IsSelection = true;
                        }
                        selectedItemCount++;
                    }
                    else if (itemBounds.IntersectsWith(this.previousArea))
                    {
                        // We previously changed the selection to true but it no
                        // longer intersects with the area so clear the selection.
                        Selector.SetIsSelected(item, false);

                        // 선택된 자식이 2개 이상이라면 임시 보관한 자식 컨트롤 선택 효과 설정
                        if (selectedItemCount == 2 && tmpSelectionChattingMessageControl != null)
                        {
                            tmpSelectionChattingMessageControl.IsSelection = true;
                        }
                        // 최초 선택 대상이라면 선택할 자식 컨트롤 임시 보관
                        if (selectedItemCount == 0)
                        {
                            tmpSelectionChattingMessageControl = chattingMessageControl;
                        }
                        else
                        {
                            chattingMessageControl.IsSelection = true;
                        }
                        selectedItemCount++;
                    }
                }
            }
            this.previousArea = area;
        }
    }

    /// <summary>
    /// 참고 - https://www.codeproject.com/Articles/209560/ListBox-drag-selection
    /// The event data for the AutoScroller.OffsetChanged event.
    /// </summary>
    private sealed class OffsetChangedEventArgs : EventArgs
    {
        private readonly double horizontal;
        private readonly double vertical;

        /// <summary>
        /// Initializes a new instance of the OffsetChangedEventArgs class.
        /// </summary>
        /// <param name="horizontal">The change in horizontal scroll.</param>
        /// <param name="vertical">The change in vertical scroll.</param>
        internal OffsetChangedEventArgs(double horizontal, double vertical)
        {
            this.horizontal = horizontal;
            this.vertical = vertical;
        }

        /// <summary>Gets the change in horizontal scroll position.</summary>
        public double HorizontalChange
        {
            get { return this.horizontal; }
        }

        /// <summary>Gets the change in vertical scroll position.</summary>
        public double VerticalChange
        {
            get { return this.vertical; }
        }
    }
}
