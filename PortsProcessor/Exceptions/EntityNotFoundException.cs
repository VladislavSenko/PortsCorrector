using System;

namespace PortsProcessor.Exceptions
{
    public class EntityNotFoundException<TEntity> : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
        }
    }
}
