public record Taxon(string TaxonID, string parentNameUsageID, //they can be this whole thing: MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea

string acceptedNameUsageID, //this is always null in the given csv file but maybe it will change later

bool taxonomicStatus, //e.g. accepted or denied. maybe this should be a string?

string taxonRank, //e.g. order, genus or family

string scientificName, //e.g. Pelecaniformes

string scientificNameAuthorship, 

string language, //which is only indicated by three letters

string vernacularName, //their dainish name

bool merged); // false, null or true
