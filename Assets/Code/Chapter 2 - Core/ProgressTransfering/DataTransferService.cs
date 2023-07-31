using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace SpinalPlay
{
    public class DataTransferService : IService
    {
        private AllData data;

        private readonly SaveLoadProgressService _progress;
        private readonly EventBus _eventBus;

        public DataTransferService(SaveLoadProgressService progress, EventBus eventBus)
        {
            _progress = progress;
            _eventBus = eventBus;

            Load();
            Subscribe();
        }

        public TData Get<TData>() where TData : DataBase, new()
        {
            if(data.progressData[typeof(TData)] == null)
            {
                data.progressData[typeof(TData)] = new TData();
            }
            return data.progressData[typeof(TData)] as TData;
        }

        public void SaveCurrent()
        {
            _progress.Save(data);
        }

        private void Load()
        {
            data = _progress.Load<AllData>(new AllData());
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<NewGame>(Reset);
        }

        private void Reset(NewGame signal)
        {
            foreach(DataBase dat in data.progressData.Values)
            {
                dat.Reset();
            }
        }
    }
}