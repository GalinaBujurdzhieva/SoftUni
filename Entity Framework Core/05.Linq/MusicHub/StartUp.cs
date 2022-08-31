namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Console.WriteLine(ExportAlbumsInfo(context, 9)); 
            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var producer = context.Producers
                .FirstOrDefault(x => x.Id == producerId);

            var albums = producer.Albums
                .Select(a => new
                {
                    AlbumName = a.Name,
                    AlbumReleaseDate = a.ReleaseDate,
                    AlbumProducerName = a.Producer.Name,
                    AlbumSongs = a.Songs
                          .Select(s => new
                          {
                              SongName = s.Name,
                              SongPrice = s.Price,
                              SongWriterName = s.Writer.Name
                          })
                          .OrderByDescending(s => s.SongName)
                         .ThenBy(s => s.SongWriterName),
                   AlbumTotalPrice = a.Price
                })
                .OrderByDescending(x => x.AlbumTotalPrice)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {album.AlbumReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}");
                sb.AppendLine($"-ProducerName: {album.AlbumProducerName}");
                sb.AppendLine("-Songs:");

                int counter = 1;
                foreach (var song in album.AlbumSongs)
                {
                    sb.AppendLine($"---#{counter++}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.SongPrice:F2}");
                    sb.AppendLine($"---Writer: {song.SongWriterName}");
                }
                sb.AppendLine($"-AlbumPrice: {album.AlbumTotalPrice:F2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs.ToList()
                .Where(x => x.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    SongName = s.Name,
                    PerformerFullName = s.SongPerformers.
                                        Select(p => p.Performer.FirstName + " " + p.Performer.LastName)
                                        .FirstOrDefault(),
                    WriterName = s.Writer.Name,
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration
                })
                .OrderBy(x => x.SongName)
                .ThenBy(x => x.WriterName)
                .ThenBy(x => x.PerformerFullName);

            StringBuilder sb = new StringBuilder();
            int counter = 1;

                foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{counter++}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.WriterName}");
                sb.AppendLine($"---Performer: {song.PerformerFullName}");
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration.ToString("c")}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
