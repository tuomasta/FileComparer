using DiffPlex;
using DiffPlex.DiffBuilder;
using Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Applications
{
    public class FileComparer : ICompareFiles
    {
        public FileDiff Diff(TextFile rightFile, TextFile leftFile)
        {
            if (rightFile?.Data == null) throw new ArgumentNullException(nameof(rightFile));
            if (rightFile?.Data == null) throw new ArgumentNullException(nameof(leftFile));

            if (rightFile.Data.Equals(leftFile.Data, StringComparison.InvariantCulture))
            {
                return new FileDiff(Enumerable.Empty<LineDiff>()) {
                    Summary = "The files are equal"
                };
            }

            var summary = ProduceSummary(rightFile, leftFile);
            var diffBuilder = new SideBySideDiffBuilder(new Differ());
            var diff = diffBuilder.BuildDiffModel(leftFile.Data, rightFile.Data);

            var changes = new List<LineDiff>();
            for (int i = 0; i < Math.Max(diff.NewText.Lines.Count, diff.OldText.Lines.Count); i++)
            {
                var newLine = diff.NewText.Lines.Count > i ? diff.NewText.Lines[i] : null;
                var oldLine = diff.OldText.Lines.Count > i ? diff.OldText.Lines[i] : null;
                var change = newLine?.Type ?? oldLine?.Type;

                if (change == DiffPlex.DiffBuilder.Model.ChangeType.Unchanged) continue;

                changes.Add(new LineDiff(newLine?.Text, oldLine?.Text, i, change.Value));
            }

            return new FileDiff(changes) {
                Summary = summary
            };
        }

        private string ProduceSummary(TextFile rightFile, TextFile leftFile)
        {
            var lenghtDiff = rightFile.Data.Length - leftFile.Data.Length;
            if (lenghtDiff == 0) return "The size of the files were equal.";

            var diff = lenghtDiff > 0 ? "larger" : "smaller";
            return $"The right file is {Math.Abs(lenghtDiff)} bytes {diff} than the left one.";
        }
    }
}