using System;
using System.Collections.Generic;
using System.Web;

using WebMatrix.Data;

public class QuoteDatabase : IDisposable
{ 
    private Database _database;
    
    public QuoteDatabase() {
        this.DatabaseName = "arealprogrammer";
        this._database = Database.Open(this.DatabaseName);
    }
    
    public QuoteDatabase(string dbName){
        this.DatabaseName = dbName;
        this._database = Database.Open(this.DatabaseName);
    }
    
    public string DatabaseName { get; private set; }
        
    public dynamic InsertQuote(string quote, string twitterUsername){
        var insertQuery = "INSERT INTO Quotes (Content, SubmittedBy, CreatedOn, IsApproved) VALUES (@0, @1, @2, @3) ";   
        
        _database.Execute(insertQuery, quote, twitterUsername, DateTime.Now, false);
        
        return _database.GetLastInsertId();
    }
    
    public dynamic GetRandomQuote(){                     
        return _database.QuerySingle("SELECT TOP 1 Id, Content, SubmittedBy FROM Quotes WHERE IsApproved = 1 ORDER BY NEWID()");          
    }
    
    public dynamic GetQuote(int quoteId){                    
        return _database.QuerySingle("SELECT Content, SubmittedBy FROM Quotes WHERE Id = @0", quoteId);           
    }
    
    public dynamic GetApprovedQuotes() { 
        return _database.Query("SELECT Id, Content, SubmittedBy FROM Quotes WHERE IsApproved = 1 ORDER BY CreatedOn DESC");  
    }  
  
    public int GetApprovedQuotesCount() {                        
        return _database.QuerySingle("SELECT COUNT(Id) AS Count FROM Quotes WHERE IsApproved = 1").Count;        
    }
    
    public void Dispose(){
        if (_database != null){
            _database.Close();
        }
    }
}
