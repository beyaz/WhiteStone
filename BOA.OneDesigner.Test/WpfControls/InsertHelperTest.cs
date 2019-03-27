using System;
using System.Collections.Generic;
using BOA.OneDesigner.Helpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.WpfControls
{
    [TestClass]
    public class InsertHelperTest
    {
        #region Public Methods
        [TestMethod]
        public void Move()
        {
            var items = new List<string> {"0", "1", "2"};

            InsertHelper.Move(items, "1", 0);

            items.Should().Equal("1", "0", "2");
        }

        [TestMethod]
        public void Move_should_add_at_the_end_of_list_when_target_index_is_bigger_than_items_count()
        {
            var items = new List<string> {"0", "1", "2"};

            InsertHelper.Move(items, "1", 3);

            items.Should().Equal("0", "2", "1");
        }

        [TestMethod]
        public void Move_should_be_fail_when_item_not_in_list()
        {
            var items = new List<string> {"0", "1", "2"};

            Action act = () => { InsertHelper.Move(items, "3", 0); };

            act.Should().Throw<BusinessException>();
        }

        [TestMethod]
        public void Move_should_be_nothing_when_indexes_are_equal()
        {
            var items = new List<string> {"0", "1", "2"};

            InsertHelper.Move(items, "0", 0);

            items.Should().Equal("0", "1", "2");
        }
        #endregion
    }
}