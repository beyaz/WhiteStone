using System;

namespace BOA.TfsAccess
{
    public class FileAccessWriteResult
    {
        public bool      ThereIsNoFileAndFileCreated                        { get; set; }
        public bool      TfsVersionAndNewContentIsSameSoNothingDoneAnything { get; set; }
        public string    CheckoutError                                      { get; set; }
        public bool      FileReadOnlyAttributeRemoved                       { get; set; }
        public Exception Exception                                          { get; set; }
    }
}