class Parse
{
    string taxonID, parentNameUsageID; //they can be this whole thing: MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea
    
    string acceptedNameUsageID; //this is always null in the given csv file but maybe it will change later
    bool taxonomicStatus; //e.g. accepted or denied. maybe this should be a string?
    
    string taxonRank; //e.g. order, genus or family
    string scientificName; //e.g. Pelecaniformes
    
    string scientificNameAuthorship; //watch out for the comma seperator here because this can be like "Montagu, 1813"
    
    /* //these we might not need since they are not in the meta.xml:
    string language; //which is only indicated by three letters
    string vernacularName; //their dainish name
    bool merged; // false, null or true
    */
    static void Parse()
    {
       
    }
    
     public void ReadTaxonID(string path, int? limit = null) {
        IEnumerable <T> objects;
        var reader = new StreamReader(path);
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        objects = csv.GetRecords<T>();

        foreach(var obj in objects)
        {
            if(obj != null)
            {
                if(true) //placeholder bool
                {
                    new Taxon();
                }
            }
        }
    }
}