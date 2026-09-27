public record Taxon(
    
string TaxonID,

string ParentNameUsageID, //they can be this whole thing: MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea

string? AcceptedNameUsageID, //this is always null in the given csv file but maybe it will change later

string TaxonomicStatus, //e.g. accepted or denied. maybe this should be a string?

string TaxonRank, //e.g. order, genus or family

string ScientificName, //e.g. Pelecaniformes

string? ScientificNameAuthorship, 

string? Language, //which is only indicated by three letters

string? VernacularName, //their dainish name

bool? Merged); // false, null or true
//