using System;

namespace BOA.OneDesigner.TfsDomain
{
    public sealed class FileAccessWriteResult
    {
        #region Public Properties
        public Exception Exception                                          { get; set; }
        public bool      TfsVersionAndNewContentIsSameSoNothingDoneAnything { get; set; }
        public bool      ThereIsNoFileAndFileCreated                        { get; set; }
        #endregion
    }
}