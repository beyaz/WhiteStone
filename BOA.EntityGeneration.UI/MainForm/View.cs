using System.Timers;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.UI.Container.Mvc;

namespace BOA.EntityGeneration.UI.MainForm
{
    public class View : WindowBase<Model, Controller>
    {
        #region Fields
        Timer timer = new Timer();
        #endregion

        #region Constructors
        public View()
        {
            this.LoadJson(EmbeddedResourceHelper.ReadFile("BOA.EntityGeneration.UI", nameof(MainForm), nameof(View) + ".json"));
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
                timer.Stop();
            }
        }

        public void StartTimer()
        {
            timer         =  new Timer(100);
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }
        #endregion

        #region Methods
        void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => { FireAction("GetCapture"); });
        }
        #endregion
    }
}