using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsMore
{
    class Ref<T>
    {
        public T Value { get; set; }

        /// <summary>
        /// Default constructor that automatically sets the default value for the T type.
        /// </summary>
        public Ref() => Value = default(T);

        /// <summary>
        /// Constructor to initialize the T type with a specific starting value.
        /// </summary>
        /// <param name="_value">Initial value for the T type</param>
        public Ref(T _value) => Value = _value;
        
        /// <summary>
        /// Shows a string representation of the T type.
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString() => Value == null ? "" : Value.ToString();

        /// <summary>
        /// Implicitly exposes the T type from the Ref<T> object.
        /// </summary>
        /// <param name="rhs">The Ref<T> object</param>
        public static implicit operator T(Ref<T> rhs) => rhs.Value;

        /// <summary>
        /// Implicitly creates a Ref<T> object from the T type.
        /// </summary>
        /// <param name="rhs">The T type</param>
        public static implicit operator Ref<T>(T rhs) => new Ref<T>(rhs);
    }
}
