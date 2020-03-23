using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Data;

namespace Frontier.Wif.Utilities.Extensions
{
    /// <summary>
    /// Defines the <see cref="CollectionViewShaper{TSource}" />
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public class CollectionViewShaper<TSource>
    {
        #region Fields

        /// <summary>
        /// Defines the _groupDescriptions
        /// </summary>
        private readonly List<GroupDescription> _groupDescriptions = new List<GroupDescription>();

        /// <summary>
        /// Defines the _sortDescriptions
        /// </summary>
        private readonly List<SortDescription> _sortDescriptions = new List<SortDescription>();

        /// <summary>
        /// Defines the _view
        /// </summary>
        private readonly ICollectionView _view;

        /// <summary>
        /// Defines the _filter
        /// </summary>
        private Predicate<object> _filter;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionViewShaper{TSource}"/> class.
        /// </summary>
        /// <param name="view">The <see cref="ICollectionView" /></param>
        public CollectionViewShaper(ICollectionView view)
        {
            if (view == null)
                throw new ArgumentNullException("view");
            _view = view;
            _filter = view.Filter;
            _sortDescriptions = view.SortDescriptions.ToList();
            _groupDescriptions = view.GroupDescriptions.ToList();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Apply
        /// </summary>
        public void Apply()
        {
            using (_view.DeferRefresh())
            {
                _view.Filter = _filter;
                _view.SortDescriptions.Clear();
                foreach (var s in _sortDescriptions) _view.SortDescriptions.Add(s);
                _view.GroupDescriptions.Clear();
                foreach (var g in _groupDescriptions) _view.GroupDescriptions.Add(g);
            }
        }

        /// <summary>
        /// The ClearAll
        /// </summary>
        /// <returns>The <see cref="CollectionViewShaper{TSource}" /></returns>
        public CollectionViewShaper<TSource> ClearAll()
        {
            _filter = null;
            _sortDescriptions.Clear();
            _groupDescriptions.Clear();
            return this;
        }

        /// <summary>
        /// The ClearFilter
        /// </summary>
        /// <returns>The <see cref="CollectionViewShaper{TSource}" /></returns>
        public CollectionViewShaper<TSource> ClearFilter()
        {
            _filter = null;
            return this;
        }

        /// <summary>
        /// The ClearGrouping
        /// </summary>
        /// <returns>The <see cref="CollectionViewShaper{TSource}" /></returns>
        public CollectionViewShaper<TSource> ClearGrouping()
        {
            _groupDescriptions.Clear();
            return this;
        }

        /// <summary>
        /// The ClearSort
        /// </summary>
        /// <returns>The <see cref="CollectionViewShaper{TSource}" /></returns>
        public CollectionViewShaper<TSource> ClearSort()
        {
            _sortDescriptions.Clear();
            return this;
        }

        /// <summary>
        /// The GroupBy
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector">The <see cref="Expression{Func{TSource, TKey}}" /></param>
        /// <returns>The <see cref="CollectionViewShaper{TSource}" /></returns>
        public CollectionViewShaper<TSource> GroupBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            var path = GetPropertyPath(keySelector.Body);
            _groupDescriptions.Add(new PropertyGroupDescription(path));
            return this;
        }

        /// <summary>
        /// The OrderBy
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector">The <see cref="Expression{Func{TSource, TKey}}" /></param>
        /// <returns>The <see cref="CollectionViewShaper{TSource}" /></returns>
        public CollectionViewShaper<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            return OrderBy(keySelector, true, ListSortDirection.Ascending);
        }

        /// <summary>
        /// The OrderByDescending
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector">The <see cref="Expression{Func{TSource, TKey}}" /></param>
        /// <returns>The <see cref="CollectionViewShaper{TSource}" /></returns>
        public CollectionViewShaper<TSource> OrderByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            return OrderBy(keySelector, true, ListSortDirection.Descending);
        }

        /// <summary>
        /// The ThenBy
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector">The <see cref="Expression{Func{TSource, TKey}}" /></param>
        /// <returns>The <see cref="CollectionViewShaper{TSource}" /></returns>
        public CollectionViewShaper<TSource> ThenBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            return OrderBy(keySelector, false, ListSortDirection.Ascending);
        }

        /// <summary>
        /// The ThenByDescending
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector">The <see cref="Expression{Func{TSource, TKey}}" /></param>
        /// <returns>The <see cref="CollectionViewShaper{TSource}" /></returns>
        public CollectionViewShaper<TSource> ThenByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            return OrderBy(keySelector, false, ListSortDirection.Descending);
        }

        /// <summary>
        /// The Where
        /// </summary>
        /// <param name="predicate">The <see cref="Func{TSource, bool}" /></param>
        /// <returns>The <see cref="CollectionViewShaper{TSource}" /></returns>
        public CollectionViewShaper<TSource> Where(Func<TSource, bool> predicate)
        {
            _filter = o => predicate((TSource) o);
            return this;
        }

        /// <summary>
        /// The GetPropertyPath
        /// </summary>
        /// <param name="expression">The <see cref="Expression" /></param>
        /// <returns>The <see cref="string" /></returns>
        private static string GetPropertyPath(Expression expression)
        {
            var names = new Stack<string>();
            var expr = expression;
            while (expr != null && !(expr is ParameterExpression) && !(expr is ConstantExpression))
            {
                var memberExpr = expr as MemberExpression;
                if (memberExpr == null)
                    throw new ArgumentException(
                            "The selector body must contain only property or field access expressions");
                names.Push(memberExpr.Member.Name);
                expr = memberExpr.Expression;
            }

            return string.Join(".", names.ToArray());
        }

        /// <summary>
        /// The OrderBy
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector">The <see cref="Expression{Func{TSource, TKey}}" /></param>
        /// <param name="clear">The <see cref="bool" /></param>
        /// <param name="direction">The <see cref="ListSortDirection" /></param>
        /// <returns>The <see cref="CollectionViewShaper{TSource}" /></returns>
        private CollectionViewShaper<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector, bool clear,
                ListSortDirection direction)
        {
            var path = GetPropertyPath(keySelector.Body);
            if (clear)
                _sortDescriptions.Clear();
            _sortDescriptions.Add(new SortDescription(path, direction));
            return this;
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="CollectionViewShaperExtensions" />
    ///     https://www.thomaslevesque.com/2011/11/30/wpf-using-linq-to-shape-data-in-a-collectionview/
    /// </summary>
    public static class CollectionViewShaperExtensions
    {
        #region Methods

        /// <summary>
        /// The Shape
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="view">The <see cref="ICollectionView" /></param>
        /// <returns>The <see cref="CollectionViewShaper{TSource}" /></returns>
        public static CollectionViewShaper<TSource> Shape<TSource>(this ICollectionView view)
        {
            return new CollectionViewShaper<TSource>(view);
        }

        /// <summary>
        /// The ShapeView
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}" /></param>
        /// <returns>The <see cref="CollectionViewShaper{TSource}" /></returns>
        public static CollectionViewShaper<TSource> ShapeView<TSource>(this IEnumerable<TSource> source)
        {
            var view = CollectionViewSource.GetDefaultView(source);
            return new CollectionViewShaper<TSource>(view);
        }

        #endregion
    }
}