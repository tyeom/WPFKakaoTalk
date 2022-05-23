using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Base;

public abstract class ViewModelBase : ObservableObject
{
    public ViewModelBase() { }

    public ViewModelBase(IView view)
    {
        View = view;
    }

    public IView View { get; private set; }

    public virtual void Cleanup()
    {
        //
    }
}

// 특정 IView 타입을 사용하는 경우 제네릭 ViewModelBase 사용
public abstract class ViewModelBase<T> : ObservableObject where T : IView
{
    public ViewModelBase(T view)
    {
        View = view;
    }

    public T View { get; private set; }

    public virtual void Cleanup()
    {
        //
    }
}
