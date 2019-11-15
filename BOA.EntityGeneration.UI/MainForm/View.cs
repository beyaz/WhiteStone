using System.Timers;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.UI.Container.Mvc;

namespace BOA.EntityGeneration.UI.MainForm
{
    public class View : WindowBase<Model, Controller>
    {
        #region Fields
        Timer _timer = new Timer();
        #endregion

        #region Constructors
        public View()
        {
            this.LoadJson(EmbeddedResourceHelper.ReadFile("BOA.EntityGeneration.UI",nameof(MainForm),nameof(View) + ".json"));
        }
        #endregion

        #region Public Methods
        public override void FireAction(string controllerPublicMethodName)
        {
            base.FireAction(controllerPublicMethodName);

            if (Model.StartTimer)
            {
                Model.StartTimer = false;
                StartTimer();
            }

            if (Model.FinishTimer)
            {
                _timer.Stop();
            }
        }

        public void StartTimer()
        {
            _timer         =  new Timer(100);
            _timer.Elapsed += OnTimedEvent;
            _timer.Start();
            
        }
        #endregion

        #region Methods
        void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                FireAction("GetCapture");           
            });
        }
        #endregion
    }
}