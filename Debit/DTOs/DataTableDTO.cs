namespace Debit.DTOs
{
    public class DataTableDTO
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public SearchDTO? Search { get; set; }
        public List<ColumnsDTO>? Order { get; set; }

        //public List<ColumnsDTO>? Columns { get; set; }

    }
    public class SearchDTO
    {
        public string? Value { get; set; }
        public bool Regex { get; set; }
    }
    public class OrderDTO
    {
        public string? Data { get; set; }
        public string? Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public SearchDTO? Search { get; set; }
    }
    public class ColumnsDTO
    {
        public int Column { get; set; }
        public string? Dir { get; set; }
    }

    public class DTData
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public object? Data { get; set; }
    }
}
