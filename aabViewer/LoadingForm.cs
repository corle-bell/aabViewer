using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace aabViewer
{
    

    public partial class LoadingForm : Form
    {
        [DllImport("user32")]
        public static extern int SetParent(int hWndChild, int hWndNewParent);


        public LoadingForm()
        {
            InitializeComponent();

            this.ControlBox = false;

        }


        public static LoadingForm instance;
        public static void ShowLoading(Form parent)
        {
            if (parent == null) return;
            if (parent.Handle == null) return;

            instance = new LoadingForm();
            instance.MdiParent = parent;
            instance.StartPosition = FormStartPosition.CenterScreen;            
            instance.Show();

            PerformStep("开始~~~");


            SetParent((int)instance.Handle, (int)parent.Handle);
        }

        public static void HideLoading()
        {
            if(instance!=null)
            {
                instance.Hide();
                instance = null;
            }
        }

        public static void PerformStep(string _text)
        {
            if(instance != null)
            {
                if (instance.InvokeRequired)//当前线程不是创建线程
                {
                    instance.Invoke(new Action(() => instance.progressBar1.PerformStep()));
                    instance.Invoke(new Action(() => instance.label1.Text = _text));
                    
                }
                else//当前线程是创建线程（界面线程）
                {
                    instance.progressBar1.PerformStep();
                    instance.label1.Text = _text;
                }   
            }
        }
    }
}
