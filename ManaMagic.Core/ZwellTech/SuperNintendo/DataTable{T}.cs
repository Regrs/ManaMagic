using System;
using System.Collections;
using System.Collections.Generic;

#nullable enable

namespace ZwellTech.SuperNintendo
{
    /// <summary>
    /// Represents a data table.
    /// </summary>
    public sealed class DataTable<T> : IEnumerable<T>
    {
        private readonly List<T> rows;

        /// <summary>
        /// Gets a <see cref="DataTable"/> with no rows.
        /// </summary>
        public static DataTable<T> Empty { get; } = new DataTable<T>(Array.Empty<T>());

        /// <summary>
        /// Gets a read-only collection of rows in the table.
        /// </summary>
        public IReadOnlyList<T> Rows { get { return this.rows; } }

        /// <summary>
        /// Gets the number of rows in the table.
        /// </summary>
        public int RowCount { get { return this.Rows.Count; } }

        /// <summary>
        /// Gets the row at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the row to get.</param>
        /// <returns>The row at the specified index.</returns>
        public T this[int index] { get { return this.Rows[index]; } }

        /// <summary>
        /// Occurs when a row is replaced.
        /// </summary>
        public event EventHandler<RowReplacedEventArgs<T>>? RowReplaced;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTable"/> with the specified rows.
        /// </summary>
        /// <param name="rows">The rows of the table.</param>
        public DataTable(IReadOnlyList<T> rows)
        {
            this.rows = new List<T>(rows);
        }

        public DataTable(List<T> rows)
        {
            this.rows = rows;
        }

        public void Replace(int index, T item)
        {
            T oldItem = this.rows[index];
            this.rows[index] = item;
            this.OnRowReplaced(index, oldItem, item);
        }

        public bool Contains(T item)
        {
            foreach (T listItem in this.Rows)
            {
                if (listItem != null && listItem.Equals(item)) { return true; }
            }
            return false;
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return Rows.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Rows).GetEnumerator();
        }

        private void OnRowReplaced(int index, T oldItem, T newItem)
        {
            RowReplacedEventArgs<T> args = new RowReplacedEventArgs<T>(index, oldItem, newItem);
            this.RowReplaced?.Invoke(this, args);
            args.Clear();
        }
    }
}