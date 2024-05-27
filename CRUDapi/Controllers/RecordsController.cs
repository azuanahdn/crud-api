using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CRUDapi.Models;

namespace CRUDapi.Controllers
{
    [ApiController]
    [Route("api/records")]
    public class RecordsController : ControllerBase
    {
        private const string DatabaseFilePath = "records.json";

        [HttpGet]
        public IActionResult GetRecords(int page = 1, string search = "", bool showDeleted = false)
        {
            try
            {
                var records = ReadRecordsFromJsonFile();
                if (!showDeleted)
                {
                    records = records.Where(r => !r.IsDeleted).ToList();
                }
                var filteredRecords = records
                    .Where(r => r.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .Skip((page - 1) * 5)
                    .Take(5)
                    .ToList();
                return Ok(filteredRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult CreateRecord([FromBody] Record record)
        {
            try
            {
                var records = ReadRecordsFromJsonFile();
                record.Id = records.Count > 0 ? records.Max(r => r.Id) + 1 : 1;
                record.CurrentTime = DateTime.UtcNow;
                records.Add(record);
                WriteRecordsToJsonFile(records);
                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRecord(int id, [FromBody] Record updatedRecord)
        {
            try
            {
                var records = ReadRecordsFromJsonFile();
                var existingRecord = records.FirstOrDefault(r => r.Id == id);
                if (existingRecord == null)
                {
                    return NotFound();
                }

                existingRecord.Name = updatedRecord.Name;
                existingRecord.Email = updatedRecord.Email;
                existingRecord.IsDeleted = updatedRecord.IsDeleted;

                WriteRecordsToJsonFile(records);
                return Ok(existingRecord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRecord(int id)
        {
            try
            {
                var records = ReadRecordsFromJsonFile();
                var existingRecord = records.FirstOrDefault(r => r.Id == id);
                if (existingRecord == null)
                {
                    return NotFound();
                }

                existingRecord.IsDeleted = true;
                WriteRecordsToJsonFile(records);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private List<Record> ReadRecordsFromJsonFile()
        {
            if (!System.IO.File.Exists(DatabaseFilePath))
            {
                return new List<Record>();
            }

            var json = System.IO.File.ReadAllText(DatabaseFilePath);
            return JsonConvert.DeserializeObject<List<Record>>(json);
        }

        private void WriteRecordsToJsonFile(List<Record> records)
        {
            var json = JsonConvert.SerializeObject(records, Formatting.Indented);
            System.IO.File.WriteAllText(DatabaseFilePath, json);
        }
    }
}
