using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aabViewer.Logcat
{
    public class DelayTextBox : TextBox
    {
        public event EventHandler DelayTextChange;
        public int DelayTime = 500;

        private long timestamp = 0;
        private System.Timers.Timer textChangeTimer;
        private string _textCache;

        private object _CacheSender;
        private EventArgs _CacheArgs;
        public DelayTextBox()
        {
            this.TextChanged += new EventHandler(this._TextChanged);

            textChangeTimer = new System.Timers.Timer(100);
            textChangeTimer.Elapsed += BatchUpdateTimer_Elapsed;
            this.Disposed += new EventHandler(this._Disposed);

            _textCache = Text;
        }


        private void BatchUpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            long now = LogcatTools.TimeStamp();
            if (now - timestamp > DelayTime)
            {
                if(this.InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        _TextChangeExec();
                    });
                }
                else
                {
                    _TextChangeExec();
                }
            }
        }

        private void _TextChangeExec()
        {
            WinformTools.Log("_TextChagne Exec Start");

            textChangeTimer.Stop();
            
            if (!_textCache.Equals(Text))
            {
                DelayTextChange?.Invoke(_CacheSender, _CacheArgs);
            }
            _textCache = Text;
            timestamp = 0;
            WinformTools.Log("_TextChagne Exec End");
        }

        private void _Disposed(object sender, EventArgs e)
        {
            _CacheSender = null;
            _CacheArgs = null;
            textChangeTimer.Stop();
        }

        private void _TextChanged(object sender, EventArgs e)
        {
            long now = LogcatTools.TimeStamp();
            if (timestamp==0)
            {
                textChangeTimer.Start();
            }
            timestamp = now;
            _CacheSender = sender;
            _CacheArgs = e;
        }

        
    }
}
