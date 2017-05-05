using System.Collections.Generic;

namespace Hangfire.Samples.Framework
{
    /// <summary>
    /// 表示 SQL 命令的類。其中包含 SQL 語句和參數兩個部分。參數的值要和 SQL 語句中的問號一一對應。 
    /// </summary>
   public class Command
    {
        private string statement;

        private List<string> @params;

        private List<string> paramTypes;

        /// <summary>
        /// 缺省構造函數
        /// </summary>
        public Command()
        {

        }

        /// <summary>
        /// 構造函數
        /// </summary>
        /// <param name="statement">SQL 語句</param>
        /// <param name="params">參數</param>
        public Command(string statement, List<string> @params)
        {
            this.statement = statement;
            this.@params = @params;
        }

        public Command(string statement, List<string> @params, List<string> paramTypes)
        {
            this.statement = statement;
            this.@params = @params;
            this.paramTypes = paramTypes;
        }

        public List<string> getParamTypes()
        {
            return paramTypes;
        }

        public void setParamTypes(List<string> paramTypes)
        {
            this.paramTypes = paramTypes;
        }

        /// <summary>
        /// 獲得 SQL 語句
        /// </summary>
        /// <returns>SQL 語句</returns>
        public string getStatement()
        {
            return statement;
        }

        /// <summary>
        /// 設置 SQL 語句
        /// </summary>
        /// <param name="statement">SQL 語句</param>
        public void setStatement(string statement)
        {
            this.statement = statement;
        }

        /// <summary>
        /// 獲得參數
        /// </summary>
        /// <returns>參數</returns>
        public List<string> getParams()
        {
            return @params;
        }

        /// <summary>
        /// 設置參數
        /// </summary>
        /// <param name="params">參數</param>
        public void setParams(List<string> @params)
        {
            this.@params = @params;
        }

        public override string ToString()
        {
            return "Command{" +
                    "statement='" + statement + '\'' +
                    ", params=" + @params +
                    '}';
        }
    }
}