using Net.Chdk.Meta.Model.Camera.Ps;
using System;
using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Meta.Writers.Camera.Ps.Props
{
    sealed class PropsPsCameraWriter : IPsCameraWriter
    {
        public void WriteCameras(string path, IDictionary<string, PsCameraData> cameras)
        {
            using (var writer = File.CreateText(path))
            {
                WriteCameras(writer, cameras);
            }
        }

        private static void WriteCameras(StreamWriter writer, IDictionary<string, PsCameraData> cameras)
        {
            foreach (var kvp in cameras)
            {
                WriteModels(writer, kvp.Key, kvp.Value);
            }
        }

        private static void WriteModels(TextWriter writer, string id, PsCameraData camera)
        {
            foreach (var model in camera.Models)
            {
                WriteRevisions(writer, id, model.Platform, model);
            }
        }

        private static void WriteRevisions(TextWriter writer, string id, string platform, PsCameraModelData model)
        {
            foreach (var kvp in model.Revisions)
            {
                WriteRevision(writer, id, platform, kvp.Key, kvp.Value.Revision);
            }
        }

        private static void WriteRevision(TextWriter writer, string id, string platform, string revisionKey, string revisionValue)
        {
            var revision = GetRevision(revisionKey);
            writer.WriteLine($"{id}-{revision}={platform}-{revisionValue}");
        }

        private static string GetRevision(string revisionKey)
        {
            var revision = Convert.ToUInt32(revisionKey, 16);
            return new string(new[] {
                (char)(((revision >> 24) & 0x0f) + 0x30),
                (char)(((revision >> 20) & 0x0f) + 0x30),
                (char)(((revision >> 16) & 0x0f) + 0x30),
                (char)(((revision >>  8) & 0x7f) + 0x60)
            });
        }
    }
}
