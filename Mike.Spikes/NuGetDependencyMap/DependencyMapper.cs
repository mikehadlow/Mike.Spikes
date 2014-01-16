using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Mike.Spikes.NuGetDependencyMap
{
    public class DependencyMapper
    {
        private const string rootPath = @"D:\Source\Pasngr.TravelStatus";
        readonly XmlSerializer serializer = new XmlSerializer(typeof(PackageConfig));

        public void Main()
        {
            var projects = FindPackageConfigFiles(rootPath)
                .Select(x => new Project
                    {
                        Name = Path.GetFileName(Path.GetDirectoryName(x)),
                        PackageConfig = GetPackageConfig(x)
                    }).ToList();

            foreach (var project in projects)
            {
                project.Dependencies = from package in project.PackageConfig.Packages
                                       from dependency in projects
                                       where dependency.Name == package.Id
                                       select dependency;

                project.Users = from user in projects
                                from package in user.PackageConfig.Packages
                                where package.Id == project.Name
                                select user;
            }

            foreach (var project in projects)
            {
                Console.Out.WriteLine(project.Name);
            }
            Console.Out.WriteLine("");

            var easyNetQProjects = projects.Where(p => p.PackageConfig.Packages.Any(c => c.Id == "EasyNetQ"));

            foreach (var easyNetQProject in easyNetQProjects)
            {
                Console.Out.WriteLine("easyNetQProject.Name = {0}", easyNetQProject.Name);
                easyNetQProject.VisitUsers(x => Console.WriteLine("\t{0}", x.Name));
                Console.Out.WriteLine("");
            }
        }

        public IEnumerable<string> FindPackageConfigFiles(string currentDirectory)
        {
            //Console.Out.WriteLine("currentDirectory = {0}", currentDirectory);
            foreach (var file in Directory.GetFiles(currentDirectory, "packages.config"))
            {
                yield return file;
            }

            foreach (var directory in Directory.GetDirectories(currentDirectory))
            {
                foreach (var children in FindPackageConfigFiles(directory))
                {
                    yield return children;
                }
            }
        }

        public PackageConfig GetPackageConfig(string packageConfigPath)
        {
            var packageConfigText = File.ReadAllText(packageConfigPath);
            return (PackageConfig)serializer.Deserialize(new StringReader(packageConfigText));
        }
    }

    public class Project
    {
        public string Name { get; set; }
        public PackageConfig PackageConfig { get; set; }
        public IEnumerable<Project> Dependencies { get; set; }
        public IEnumerable<Project> Users { get; set; } 

        public void VisitDependencies(Action<Project> projectVisitor)
        {
            projectVisitor(this);
            foreach (var dependency in Dependencies)
            {
                dependency.VisitDependencies(projectVisitor);
            }
        }

        public void VisitUsers(Action<Project> projectVisitor)
        {
            projectVisitor(this);
            foreach (var user in Users)
            {
                user.VisitUsers(projectVisitor);
            }
        }
    }

    [XmlRoot("packages")]
    public class PackageConfig
    {
        [XmlElement("package")]
        public Package[] Packages { get; set; }
    }

    public class Package
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlAttribute("targetFramework")]
        public string TargetFramework { get; set; }
    }
}