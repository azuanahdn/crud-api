using CRUDapi.Models;
using System.Collections.Generic;

namespace CRUDapi.Services
{
    public interface IDataService
    {
        List<Record> GetAllRecords();
        Record GetRecordById(int id);
        void AddRecord(Record record);
        void UpdateRecord(Record record);
        void DeleteRecord(int id);
    }
}
