using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Exiled.API.Features;

namespace Fentanyl_ReactorUpdate.API.Database;

public class LiteSQL
{
    private static SQLiteConnection _connection;
    private static SQLiteCommand _command;

    /// <summary>
    /// Stellt eine Verbindung zur SQLite-Datenbank her.
    /// Erstellt die Datenbankdatei, wenn sie nicht existiert.
    /// </summary>
    public static void Connect()
    {
        try
        {
            // Prüfen, ob die Datenbankdatei existiert
            string databaseFilePath = "datenbank.db";
            if (!File.Exists(databaseFilePath))
            {
                SQLiteConnection.CreateFile(databaseFilePath);
                Log.Info("Datenbankdatei wurde erstellt.");
            }

            // Verbindung zur Datenbank herstellen
            string connectionString = $"Data Source={databaseFilePath};Version=3;";
            _connection = new SQLiteConnection(connectionString);
            _connection.Open();
            _command = _connection.CreateCommand();

            Log.Info("Verbindung zur SQLite-Datenbank hergestellt.");
            SQLManager.OnCreate(); // Tabellen initialisieren
        }
        catch (Exception ex)
        {
            Log.Info($"Fehler beim Verbinden: {ex.Message}");
        }
    }

    /// <summary>
    /// Trennt die Verbindung zur SQLite-Datenbank.
    /// </summary>
    public static void Disconnect()
    {
        try
        {
            if (_connection != null)
            {
                _connection.Close();
                Log.Info("Verbindung zur SQLite-Datenbank getrennt.");
            }
        }
        catch (Exception ex)
        {
            Log.Info($"Fehler beim Trennen der Verbindung: {ex.Message}");
        }
    }

    /// <summary>
    /// Führt ein SQL-Update (ohne Rückgabe) aus.
    /// </summary>
    public static void OnUpdate(string sql)
    {
        try
        {
            _command.CommandText = sql;
            _command.ExecuteNonQuery();
            Log.Info("SQL-Update erfolgreich ausgeführt.");
        }
        catch (Exception ex)
        {
            Log.Info($"Fehler beim SQL-Update: {ex.Message}");
        }
    }

    /// <summary>
    /// Führt ein SQL-Update ohne Fehlerbehandlung aus (wirft Exceptions).
    /// </summary>
    public static void OnUpdateRaw(string sql)
    {
        _command.CommandText = sql;
        _command.ExecuteNonQuery();
    }

    /// <summary>
    /// Führt eine SQL-Abfrage aus und gibt ein DataTable mit den Ergebnissen zurück.
    /// </summary>
    public static DataTable OnQuery(string sql)
    {
        try
        {
            var dataTable = new DataTable();
            _command.CommandText = sql;
            using (var reader = _command.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            Log.Info("SQL-Abfrage erfolgreich ausgeführt.");
            return dataTable;
        }
        catch (Exception ex)
        {
            Log.Info($"Fehler bei der SQL-Abfrage: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Führt eine SQL-Abfrage ohne Fehlerbehandlung aus (wirft Exceptions).
    /// </summary>
    public static DataTable OnQueryRaw(string sql)
    {
        var dataTable = new DataTable();
        _command.CommandText = sql;
        using (var reader = _command.ExecuteReader())
        {
            dataTable.Load(reader);
        }

        return dataTable;
    }
}
