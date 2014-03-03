using System;
using System.Linq;
using System.IO;
using System.Text;

namespace Mike.Spikes.WholeCaboodleHelpers
{
    public class ConflictFixing
    {
        private const string root = @"D:\ba1\devSrc";
        private const string conflicts = @"Conflicts.txt";
        private const string resolved = @"Resolved.txt";

        public void ResolveConflicts()
        {
            var path = Path.Combine(root, conflicts);
            var conflictList = File.ReadAllText(path)
                .Split(Environment.NewLine.ToCharArray())
                .Select(x => x.Trim())
                .Where(x => x.Length > 0);

            var count = 0;
            var builder = new StringBuilder();

            foreach (var conflictPath in conflictList)
            {
                var file = File.ReadAllText(Path.Combine(root, conflictPath));
                if(file.Contains(searchString))
                {
                    builder.AppendLine(conflictPath);

//                    var updatedFile = file.Replace(searchString, replacement);
//                    File.WriteAllText(Path.Combine(root, conflictPath), updatedFile);

                    count++;
                }

            }

            //File.WriteAllText(Path.Combine(root, resolved), builder.ToString());
            // original total 91

            Console.Out.WriteLine(builder.ToString());
            Console.Out.WriteLine("\ncount = {0}", count);
        }

        public void ShowResolvedFiles()
        {
            var path = Path.Combine(root, resolved);
            var resolvedList = File.ReadAllText(path)
                .Split(Environment.NewLine.ToCharArray())
                .Select(x => x.Trim())
                .Where(x => x.Length > 0);

            foreach (var resolvedFile in resolvedList)
            {
                var file = File.ReadAllText(Path.Combine(root, resolvedFile));
                Console.Out.WriteLine(file);
                Console.Out.WriteLine("\n");
            }
        }

        private const string searchString = 
@"<<<<<<< HEAD
  <package id=""RabbitMQ.Client"" version=""3.1.5"" targetFramework=""net45"" />
</packages>
=======
  <package id=""RabbitMQ.Client"" version=""3.1.1"" targetFramework=""net45"" />
</packages>
>>>>>>> a78d966bf65371c97f4a99d6edb2a1c753e05f69";

        private const string replacement =
@"<package id=""RabbitMQ.Client"" version=""3.1.5"" targetFramework=""net45"" />
</packages>";
    }
}