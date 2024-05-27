using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CRUDapi.Models;
using Microsoft.Extensions.Options;

namespace CRUDapi.Services
{
    public class JsonFileDataService : IDataService
    {
        private readonly string _filePath;

        public JsonFileDataService(IOptions<JsonFileOptions> jsonFileOptions)
        {
            _filePath = jsonFileOptions.Value.FilePath;
        }

        public List<Record> GetAllRecords()
        {
            using var fileStream = File.OpenRead(_filePath);
            return JsonSerializer.Deserialize<List<Record>>(fileStream) ?? new List<Record>();
        }

        public Record GetRecordById(int id)
        {
            return GetAllRecords().FirstOrDefault(r => r.Id == id);
        }

        public void AddRecord(Record record)
        {
            var records = GetAllRecords();
            record.Id = records.Any() ? records.Max(r => r.Id) + 1 : 1;
            records.Add(record);
            SaveRecords(records);
        }

        public void UpdateRecord(Record record)
        {
            var records = GetAllRecords();
            var existingRecord = records.FirstOrDefault(r => r.Id == record.Id);
            if (existingRecord != null)
            {
                existingRecord.Name = record.Name;
                existingRecord.Email = record.Email;
                existingRecord.IsDeleted = record.IsDeleted;
                SaveRecords(records);
            }
        }

        public void DeleteRecord(int id)
        {
            var records = GetAllRecords();
            var recordToDelete = records.FirstOrDefault(r => r.Id == id);
            if (recordToDelete != null)
            {
                records.Remove(recordToDelete);
                SaveRecords(records);
            }
        }

        private void SaveRecords(List<Record> records)
        {
            File.WriteAllText(_filePath, JsonSerializer.Serialize(records));
        }
    }
}
