using System;

namespace BOA.TfsAccess
{
    public class FileAccessWriteResult
    {
        #region Public Properties
        public string    CheckoutError                                      { get; set; }
        public Exception Exception                                          { get; set; }
        public bool      FileIsCheckOutFromTfs                              { get; set; }
        public bool      FileReadOnlyAttributeRemoved                       { get; set; }
        public bool      TfsVersionAndNewContentIsSameSoNothingDoneAnything { get; set; }
        public bool      ThereIsNoFileAndFileCreated                        { get; set; }
        #endregion
    }
}