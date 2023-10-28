using MongoDB.Driver;
using MongoDB.Bson;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "mongodb+srv://sa:MongoDB123456@clusterteste.p1bpbaw.mongodb.net/?retryWrites=true&w=majority";

        MongoClient client = new MongoClient(connectionString);

        IMongoDatabase database = client.GetDatabase("BancoGames");
        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("games");

        for (int i = -1; i != 0;)
        {
            Console.WriteLine("_______________________________");
            Console.WriteLine("CRUD Gamelis - MongoDB");
            Console.WriteLine("1 - Adicionar game");
            Console.WriteLine("2 - Listar games");
            Console.WriteLine("3 - Atualizar game");
            Console.WriteLine("4 - Deletar game");
            Console.WriteLine("5 - Adicionar lista de games");
            Console.WriteLine("0 - Sair");

            i = int.Parse(Console.ReadLine());

            if (i == 1)
                CreateDocument(collection);
            else if (i == 2)
                ListDocument(collection);
            else if (i == 3)
                UpdateDocument(collection);
            else if (i == 4)
                DeleteDocument(collection);
            else if (i == 5)
                CreateListDocument(collection);
            else if (i == 0)
                break;
            else
                Console.WriteLine("Opção inválida");
        }
    }

    private static void CreateDocument(IMongoCollection<BsonDocument> collection)
    {
        Console.WriteLine("Digite o nome do game:");
        string? name = Console.ReadLine();

        Console.WriteLine("Digite o ano de lançamento do game:");
        int year = int.Parse(Console.ReadLine());

        Console.WriteLine("Digite a dificuldade do game:");
        string? difficulty = Console.ReadLine();

        BsonDocument document = new()
        {
            { "name", name },
            { "year", year },
            { "difficulty", difficulty }
        };

        collection.InsertOne(document);

        Console.WriteLine("Game adicionado com sucesso");
        Console.ReadKey();
        Console.Clear();
    }

    private static void CreateListDocument(IMongoCollection<BsonDocument> collection)
    {
        Console.WriteLine("Digite quantos games serão inseridos:");
        int qtdGames = int.Parse(Console.ReadLine());

        List<BsonDocument> list = new List<BsonDocument>();
        for (int i = 1; i <= qtdGames; i++)
        {
            Console.WriteLine($"Digite o nome do game {i}:");
            string? name = Console.ReadLine();

            Console.WriteLine($"Digite o ano de lançamento do game {i}:");
            int year = int.Parse(Console.ReadLine());

            Console.WriteLine($"Digite a dificuldade do game {i}:");
            string? difficulty = Console.ReadLine();

            BsonDocument document = new()
            {
                { "name", name },
                { "year", year },
                { "difficulty", difficulty }
            }; 

            list.Add(document);
        }

        collection.InsertMany(list);

        Console.WriteLine("Games adicionados com sucesso");
        Console.ReadKey();
        Console.Clear();
    }

    private static void ListDocument(IMongoCollection<BsonDocument> collection)
    {
        var filterList = Builders<BsonDocument>.Filter.Empty;
        var resultList = collection.Find(filterList).ToList();

        foreach (var documentList in resultList)
        {
            Console.WriteLine(documentList.ToJson());
        }

        Console.ReadKey();
        Console.Clear();
    }

    private static void UpdateDocument(IMongoCollection<BsonDocument> collection)
    {
        Console.WriteLine("Digite o nome do game para ser atualizado:");
        string? name = Console.ReadLine();
        var filterUpd = Builders<BsonDocument>.Filter.Eq("name", name);

        Console.WriteLine("Digite o campo a ser atualizado:");
        string? campo = Console.ReadLine();
        Console.WriteLine($"Digite o novo valor de {campo} do game {name}");
        string? newValue = Console.ReadLine();

        var update = Builders<BsonDocument>.Update.Set(campo, newValue);
        var resultUpd = collection.UpdateOne(filterUpd, update);
        Console.WriteLine($"Registros atualizados: {resultUpd.ModifiedCount}");

        Console.ReadKey();
        Console.Clear();
    }

    private static void DeleteDocument(IMongoCollection<BsonDocument> collection)
    {
        Console.WriteLine("Digite o campo a ser filtrado:");
        string? campo = Console.ReadLine();

        Console.WriteLine("Digite o valor a ser filtrado:");
        string? value = Console.ReadLine();

        var filterDel = Builders<BsonDocument>.Filter.Eq(campo, value);
        var resultDel = collection.DeleteOne(filterDel);
        Console.WriteLine($"Registros deletados: {resultDel.DeletedCount}");

        Console.ReadKey();
        Console.Clear();
    }
}