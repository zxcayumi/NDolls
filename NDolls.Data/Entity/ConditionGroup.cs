using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data.Entity
{
    /// <summary>
    /// 查询项-条件组(组内条件为OR关系)
    /// </summary>
    public class ConditionGroup : Item
    {
        private List<ConditionItem> groupConditions = new List<ConditionItem>();

        public ConditionGroup()
        {
            this.ItemType = Entity.ItemType.ConditionGroup;
        }

        /// <summary>
        /// 添加条件项
        /// </summary>
        /// <param name="item"></param>
        public void AddCondition(ConditionItem item)
        {
            groupConditions.Add(item);
        }

        public override void LoadParameters(StringBuilder sb, List<System.Data.Common.DbParameter> pars, JoinType joinType)
        {
            if (sb.Length > 0)
            {
                sb.Append(" AND ");
            }

            sb.Append("(");
            foreach (ConditionItem item in GroupConditions)
            {
                item.LoadParameters(sb, pars, joinType);
            }
            sb.Append(")");
        }

        /// <summary>
        /// 组条件项集合
        /// </summary>
        public List<ConditionItem> GroupConditions
        {
            get { return groupConditions; }
            set { groupConditions = value; }
        }
    }
}
