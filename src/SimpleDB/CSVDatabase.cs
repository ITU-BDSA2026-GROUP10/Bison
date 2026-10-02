namespace SimpleDB;

using CsvHelper;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq.Expressions;
using System.Security.AccessControl;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using CsvHelper.Configuration;
using taxons;

sealed public class CSVDatabase<T> : IDatabaseRepository<T> 
{

    private static readonly CSVDatabase<T> instance = new CSVDatabase<T>();

    static CSVDatabase() {} 

    private CSVDatabase() {} 

    TreeBuilder tb = TreeBuilder.getInstance();
    public static CSVDatabase<T> getInstance() 
    {
        return instance;
    }

    public IEnumerable<T> ReadObservation(string path, int? limit = null) {
        IEnumerable <T> objects;
        StreamReader reader = new StreamReader(path);
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        objects = csv.GetRecords<T>();
        return objects;
    }


    public IEnumerable<T> ReadDiscussion(string path, long observationId, int? limit = null)
    {
        IEnumerable<T> objects;
        var reader = new StreamReader(path);
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        objects = csv.GetRecords<T>();

        List<T> comments = new List<T>();
        foreach (var obj in objects)
        {
            if (obj is Comment comment && obj != null)
            {
                long id = comment.ObservationId;
                if (id == observationId)
                {
                    comments.Add(obj);
                }
            }
        }
        return comments;
    }

    public IEnumerable<T> ReadProposals(string path, long observationId, int? limit = null)
    {
        IEnumerable<T> objects;
        var reader = new StreamReader(path);
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        objects = csv.GetRecords<T>();

        List<T> proposals = new List<T>();
        foreach (var obj in objects)
        {
            if (obj is Proposal proposal && obj != null)
            {
                long id = proposal.ObservationId;
                if (id == observationId)
                {
                    proposals.Add(obj);
                }
            }
        }
        return proposals;
    }

    public IEnumerable<Taxon>  ReadTaxon()
    {
        Console.WriteLine("reading taxons");

        var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
        using var reader = embeddedProvider.GetFileInfo("joined.csv").CreateReadStream();
        using var sr = new StreamReader(reader);
        var csv = new CsvReader(sr, CultureInfo.InvariantCulture);
        IEnumerable<Taxon> taxons = csv.GetRecords<Taxon>();
        
        List<Taxon> taxonsList = taxons.ToList<Taxon>();

        tb.mapTaxonPairings(taxonsList);
        tb.build(taxonsList);

        return taxonsList;
    }
 
    public void Store(T record, string path, string location) {
        using (StreamWriter writer = File.AppendText(path))
        {
            if(record is Observations ob && record != null)
            {
                writer.WriteLine(ob.Author + ",\"" + ob.Observation + "\"," + ob.Timestamp + "," + ob.ID + "," + ob.Location);
            }
            writer.Close();
        }
    }

    public void StoreComment(T record, string observePath, string commentPath, long ObservationID)
    {
        long count = Counter(observePath);
        try{
            if(count >= ObservationID)
            {
                using (StreamWriter writer = File.AppendText(commentPath))
                {
                    if(record is Comment com && record != null)
                    {
                        writer.WriteLine(com.Author + ",\"" +  com.Observation + "\"," + com.Timestamp + "," + com.ObservationId);
                    }
                    
                    writer.Close();
                }
            } else
            {
                throw new ArgumentException("The observation does not exist");
            }
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void StoreProposal(T record, string proposalPath, string observePath, long ObservationID)
    {
        if(record is Proposal pro && record != null)
        {   
            long count = Counter(observePath);
            
            try{
                var set = new HashSet<string>(tb.getIdToTaxonDictionary().Keys);
                if(!set.Contains(pro.TaxonId))
                {
                    throw new ArgumentException("The taxonId does not exist");
                }  
                if(count >= ObservationID)
                {
                    using (StreamWriter writer = File.AppendText(proposalPath))
                    {
                        
                        writer.WriteLine(pro.Author + ",\"" +  pro.TaxonId + "\"," + pro.Timestamp + "," + pro.ObservationId);
                        writer.Close();
                    }
                } else
                {
                    throw new ArgumentException("The observation does not exist");
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
    
    //Used https://github.com/JoshClose/CsvHelper/issues/948 as reference
   private long Counter(string path)
   {
       using(StreamReader reader = new StreamReader(path))
       {
           int recordsLength = 0;
           while(reader.ReadLine() != null)
           {
               ++recordsLength;
           }
           recordsLength--; // line 1 doesn't count because there are column headers in first row.
           return recordsLength;
       }
   }

   public long GetNumberOfLinesInAFile(string path)
    {
        return Counter(path);
    }
}