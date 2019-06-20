﻿using System.IO;
using System.Timers;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.UI.Container.Mvc;

namespace CustomSqlInjectionToProject.MainForm
{
    public class View : WindowBase<Model, Controller>
    {
        #region Fields
        Timer _timer = new Timer();
        #endregion

        #region Constructors
        public View()
        {
            var dir = Path.GetDirectoryName(GetType().Assembly.Location) + Path.DirectorySeparatorChar;
            this.LoadJsonFile(dir + nameof(MainForm) + Path.DirectorySeparatorChar + nameof(View) + ".json");
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
            Dispatcher.Invoke(() => { FireAction("GetCapture"); });
        }
        #endregion
    }
}