using System;
using SQLite;

namespace NEC.ESBU.Ticketing.Client.ValidatorBase.LocalStorage
{
    public abstract class LocalStorageBaseModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        protected LocalStorageBaseModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
