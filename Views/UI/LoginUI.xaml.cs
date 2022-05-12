using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Views.UI;
/// <summary>
/// LoginUI.xaml에 대한 상호 작용 논리
/// </summary>
public partial class LoginUI : UserControl
{
    public LoginUI()
    {
        InitializeComponent();
    }

    private byte[] ConvertSecureStringToByteArray(SecureString value)
    {
        //Byte array to hold the return value
        byte[] returnVal = new byte[value.Length];

        IntPtr valuePtr = IntPtr.Zero;
        try
        {
            valuePtr = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(value);
            for (int i = 0; i < value.Length; i++)
            {
                short unicodeChar = System.Runtime.InteropServices.Marshal.ReadInt16(valuePtr, i * 2);
                returnVal[i] = Convert.ToByte(unicodeChar);
            }

            return returnVal;
        }
        finally
        {
            System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
        }
    }

    private void xUserIDHint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        this.xUserID.Focus();
    }

    private void xPassword_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if(this.xPassword.SecurePassword.Length > 0)
        {
            this.xPasswordHint.Visibility = Visibility.Collapsed;
        }
        else
        {
            this.xPasswordHint.Visibility = Visibility.Visible;
        }

        byte[] bytePassword = this.ConvertSecureStringToByteArray(this.xPassword.SecurePassword);
        this.xPassword.Tag = bytePassword;
    }

    private void xPasswordHint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        this.xPassword.Focus();
    }
}
