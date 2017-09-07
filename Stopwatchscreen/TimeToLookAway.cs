using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _20Min.Annotations;

namespace _20Min
{
    public class TimeToLookAway : IDisposable, INotifyPropertyChanged
    {
        private readonly Timer _t;
        private TimeSpan _restTime;
        private string _value;

        public TimeToLookAway()
        {
            this._restTime = TimeSpan.FromMinutes(20);
            this.Value = $"Encore {this._restTime.TotalMinutes} min";
            this._t = new Timer((state) =>
            {
                this._restTime = _restTime.Subtract(TimeSpan.FromMinutes(1));
                this.Value = $"Encore {this._restTime.TotalMinutes} min";
                if (_restTime.TotalMinutes <= 0)
                {
                    this.Value = "Regarde ailleurs !!!";
                    this.RaiseLookAway();
                    this._restTime = TimeSpan.FromMinutes(20);
                }
            }, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
        }

        private void RaiseLookAway()
        {
            if (LookAway != null)
            {
                this.LookAway(this, new EventArgs());
            }
        }

        public event EventHandler LookAway;

        public string Value
        {
            get { return this._value; }
            set
            {
                this._value = value;
                this.OnPropertyChanged();
            }
        }

        public void Dispose()
        {
            this._t.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}