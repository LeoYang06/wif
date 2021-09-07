/**************************************************************************
*      File Name：EnumCollection.cs
*    Description：EnumCollection.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Collections.ObjectModel;

namespace Frontier.Wif.Core.Collections
{
    /// <summary>
    /// Defines the <see cref="EnumCollection" />
    /// </summary>
    public class EnumCollection : ObservableCollection<Enum>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumCollection"/> class.
        /// </summary>
        public EnumCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumCollection"/> class.
        /// </summary>
        /// <param name="values">The <see cref="Enum[]" /></param>
        public EnumCollection(params Enum[] values) : base(values)
        {
        }

        #endregion
    }
}