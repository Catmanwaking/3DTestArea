using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Matrices
{
    /// <summary>
    /// The exception that is thrown when an attempt is made to use arithmatic on two matrices of unfit sizes.
    /// </summary>
    [Serializable]
    public class MatrixDimensionMismatchException : Exception
    {
        public string ResourceReferenceProperty { get; set; }

        public MatrixDimensionMismatchException() : base() { }

        public MatrixDimensionMismatchException(string message) : base(message) { }

        public MatrixDimensionMismatchException(string message, Exception inner) : base(message, inner) { }

        protected MatrixDimensionMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            info.AddValue("ResourceReferenceProperty", ResourceReferenceProperty);
            base.GetObjectData(info, context);
        }
    }
}
