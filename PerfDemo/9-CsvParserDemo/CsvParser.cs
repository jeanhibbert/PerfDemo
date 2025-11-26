using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PerfDemo._9_CsvParserDemo;

public readonly record struct SalaryRecord(
    int WorkYear,
    string ExperienceLevel,
    string EmploymentType,
    string JobTitle,
    int Salary,
    string SalaryCurrency,
    int SalaryInUsd,
    string EmployeeResidence,
    int RemoteRatio,
    string CompanyLocation,
    string CompanySize
);

public class CsvParser
{
    public static void ParseCsv1()
    {
        var csvContent = File.ReadAllText("salaries-2024.csv");

        var lines = csvContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        //var records = new List<SalaryRecord>();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split(',');

            var record = new SalaryRecord(
                WorkYear: int.Parse(fields[0]),
                ExperienceLevel: fields[1],
                EmploymentType: fields[2],
                JobTitle: fields[3],
                Salary: int.Parse(fields[4]),
                SalaryCurrency: fields[5],
                SalaryInUsd: int.Parse(fields[6]),
                EmployeeResidence: fields[7],
                RemoteRatio: int.Parse(fields[8]),
                CompanyLocation: fields[9],
                CompanySize: fields[10]
            );

        
        }

        //SaveRecordsAsJson(records, "salaries_2024_1.json", writeIndented: true);
    }

    public static void ParseCsv2()
    {
        var lines = File.ReadAllLines("salaries-2024.csv");
        //var records = new List<SalaryRecord>();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split(',');

            var record = new SalaryRecord(
                WorkYear: int.Parse(fields[0]),
                ExperienceLevel: fields[1],
                EmploymentType: fields[2],
                JobTitle: fields[3],
                Salary: int.Parse(fields[4]),
                SalaryCurrency: fields[5],
                SalaryInUsd: int.Parse(fields[6]),
                EmployeeResidence: fields[7],
                RemoteRatio: int.Parse(fields[8]),
                CompanyLocation: fields[9],
                CompanySize: fields[10]
            );


        }

        //SaveRecordsAsJson(records, "salaries_2024_1.json", writeIndented: true);
    }

    public static void ParseCsv3()
    {

        IEnumerable<string> lines = File.ReadLines("salaries-2024.csv");
        
        foreach (var line in lines.Skip(1))
        { // Skip the Header
            string[] fields = line.Split(',');

            var record = new SalaryRecord(
                WorkYear: int.Parse(fields[0]),
                ExperienceLevel: fields[1],
                EmploymentType: fields[2],
                JobTitle: fields[3],
                Salary: int.Parse(fields[4]),
                SalaryCurrency: fields[5],
                SalaryInUsd: int.Parse(fields[6]),
                EmployeeResidence: fields[7],
                RemoteRatio: int.Parse(fields[8]),
                CompanyLocation: fields[9],
                CompanySize: fields[10]
            );
        }

        //SaveRecordsAsJson(records, "salaries_2024_2.json", writeIndented: true);
    }

    public static void ParseCsv4()
    {
        byte[] bytes = File.ReadAllBytes("salaries-2024.csv");
        ReadOnlySpan<byte> span = bytes;

        bool first = true;
        foreach (Range range in span.Split((byte)'\n'))
        {
            if (first)
            {
                first = false;
                continue;
            }
            ReadOnlySpan<byte> line = span[range];

            SalaryRecord record = CreateSalaryRecordFromSpan(line);
        }

        //SaveRecordsAsJson(records, "salaries_2024_3.json", writeIndented: true);
    }

    internal static void ParseCsv5()
    {
        using Stream stream = File.OpenRead("salaries-2024.csv");
        var length = (int)stream.Length;

        using var memoryOwner = MemoryPool<byte>.Shared.Rent(length);

        var span = memoryOwner.Memory.Span.Slice(0, length); // 'remove' overcapacity
        stream.ReadExactly(span);
        ReadOnlySpan<byte> roSpan = span; // Use it readonly so we can use Split()

        bool first = true;
        foreach (Range range in roSpan.Split((byte)'\n'))
        {
            if (first)
            {
                first = false;
                continue;
            }
            ReadOnlySpan<byte> line = span[range];

            SalaryRecord record = CreateSalaryRecordFromSpan(line);
        }

        //SaveRecordsAsJson(records, "salaries_2024_4.json", writeIndented: false);
    }

    private static SalaryRecord CreateSalaryRecordFromSpan(ReadOnlySpan<byte> line)
    {
        int workYear = default;
        string experienceLevel = default!;
        string employmentType = default!;
        string jobTitle = default!;
        int salary = default;
        string salaryCurrency = default!;
        int salaryInUsd = default;
        string employeeResidence = default!;
        int remoteRatio = default;
        string companyLocation = default!;
        string companySize = default!;

        int t = 0;
        foreach (Range range in line.Split((byte)','))
        {
            ReadOnlySpan<byte> value = line[range];

            switch (t++)
            {
                case 0:
                    Utf8Parser.TryParse(value, out workYear, out var _);
                    break;
                case 1:
                    experienceLevel = Encoding.UTF8.GetString(value);
                    break;
                case 2:
                    employmentType = Encoding.UTF8.GetString(value);
                    break;
                case 3:
                    jobTitle = Encoding.UTF8.GetString(value);
                    break;
                case 4:
                    Utf8Parser.TryParse(value, out salary, out var _);
                    break;
                case 5:
                    salaryCurrency = Encoding.UTF8.GetString(value);
                    break;
                case 6:
                    Utf8Parser.TryParse(value, out salaryInUsd, out var _);
                    break;
                case 7:
                    employeeResidence = Encoding.UTF8.GetString(value);
                    break;
                case 8:
                    Utf8Parser.TryParse(value, out remoteRatio, out var _);
                    break;
                case 9:
                    companyLocation = Encoding.UTF8.GetString(value);
                    break;
                case 10:
                    companySize = Encoding.UTF8.GetString(value);
                    break;
                default:
                    break;
            }
        }

        return new SalaryRecord(
            WorkYear: workYear,
            ExperienceLevel: experienceLevel,
            EmploymentType: employmentType,
            JobTitle: jobTitle,
            Salary: salary,
            SalaryCurrency: salaryCurrency,
            SalaryInUsd: salaryInUsd,
            EmployeeResidence: employeeResidence,
            RemoteRatio: remoteRatio,
            CompanyLocation: companyLocation,
            CompanySize: companySize
        );
    }

    
    private static void SaveRecordsAsJson(IEnumerable<SalaryRecord> records, string outputPath, bool writeIndented = true)
    {
        var options = new JsonSerializerOptions { WriteIndented = writeIndented };
        string json = JsonSerializer.Serialize(records, options);
        File.WriteAllText(outputPath, json);
    }
}

