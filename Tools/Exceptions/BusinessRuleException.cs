using System;
using System.Collections.Generic;

namespace Core.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException() : base() { }

        public BusinessRuleException(string message) : base(message)
        {
            Message = FriendlyMessage(message);
        }

        public BusinessRuleException(string message, Exception inner) : base(message, inner)
        {
            Message = FriendlyMessage(message, inner);
        }

        protected BusinessRuleException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }

        public override string Message { get; }

        private string FriendlyMessage(string message, Exception inner = null)
        {
            var errors = Errors();
            foreach (var error in errors.Keys)
            {
                if (message.ToLower().Contains(error.ToLower()))
                {
                    return errors[error];
                }
            }
            if (inner==null) return message;

            foreach (var error in errors.Keys)
            {
                if (inner.Message.ToLower().Contains(error.ToLower()) || (inner.InnerException != null && inner.InnerException.Message.ToLower().Contains(error.ToLower())))
                {
                    return errors[error];
                }
            }
            return message;
        }

        private Dictionary<string, string> Errors()
        {
            var errors = new Dictionary<string, string>
            {
                {"FOREIGN KEY", "Esse item possui registros associados a ele."},
                {"REFERENCE", "Esse item possui registros associados a ele."},
                {"COULD NOT DELETE", "Esse item possui registros associados a ele."},
                {"DUPLICATE KEY", "Já existe um registro com as mesmas informação deste item."}
            };

            return errors;
        }
    }
}