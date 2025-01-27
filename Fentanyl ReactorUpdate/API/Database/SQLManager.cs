using System;
using System.Collections.Generic;
using System.IO;

namespace Fentanyl_ReactorUpdate.API.Database;

public class SQLManager
{
    /// <summary>
    /// Erstellt die notwendigen Tabellen in der SQLite-Datenbank, wenn sie nicht existieren.
    /// </summary>
    public static void OnCreate()
    {
        // Beispiel: Tabellen erstellen
        LiteSQL.OnUpdate(
            "CREATE TABLE IF NOT EXISTS Erfolge(" +
            "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
            "ErfolgName TEXT, " +
            "UserID INTEGER, " +
            "messageid INTEGER, " +
            "rollenid INTEGER, " +
            "color TEXT)"
        );

        // Hier können weitere Tabellen erstellt werden, z.B.:
        // LiteSQL.OnUpdate("CREATE TABLE IF NOT EXISTS ...");
    }
}