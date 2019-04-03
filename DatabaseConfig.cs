using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DatabaseConfig
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType = DataBaseType.MySql;

        /// <summary>
        /// 主机ip或地址
        /// </summary>
        public string Host;

        /// <summary>
        /// 端口
        /// </summary>
        public int Port = 1433;

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName = "";

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord = "";

        /// <summary>
        /// 数据库
        /// </summary>
        public string Database = "";

        /// <summary>
        /// 是否windows验证
        /// </summary>
        public bool WindowsVerify = false;

        /// <summary>
        /// 生成链接语句
        /// </summary>
        /// <returns></returns>
        public string GetConnectStr()
        {
            string connect = "";
            switch (DataBaseType)
            {
                case DataBaseType.Sqlite:
                    return SqliteConn(Host);
                case DataBaseType.MySql:
                    return MySqlConn(Host, UserName, PassWord, Database, "utf8", Port.ToString());
                case DataBaseType.SqlServer:
                    if (string.IsNullOrEmpty(Database)) Database = "master";
                    if (this.WindowsVerify) return string.Format("Integrated Security=SSPI;Data Source={0};Initial Catalog ={1};", Host, Database);
                    else
                    {
                        return string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", Host, Database, UserName, PassWord);
                    }
            }
            return connect;
        }

        /// <summary>
        /// SqlServer 连接语句
        /// </summary>
        /// <param name="database">数据库名</param>
        /// <param name="user">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="server">数据库路径</param>
        /// <returns></returns>
        public static string SqlServerConn(string server, string user, string password, string database)
        {
            database = database == "" ? "master" : database;
            return string.Format("Provider=SQLOLEDB;Data Source={0};User ID={1};Password={2};Initial Catalog={3};Connect Timeout=15", server, user, password, database);
        }

        /// <summary>
        /// Mysql连接语句
        /// </summary>
        /// <param name="server">服务器</param>
        /// <param name="user">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="database">数据库</param>
        /// <param name="charset">编码utf8</param>
        /// <param name="port">端口</param>
        /// <returns></returns>
        public static string MySqlConn(string server, string user, string password, string database, string charset, string port)
        {
            if (string.IsNullOrEmpty(database)) database = "information_schema";
            return "server=" + server + ";user id=" + user + "; password=" + password + "; database=" + database + "; pooling=true;charset=" + charset + ";Port=" + port + ";";
        }

        /// <summary>
        /// Sqlite连接语句,不包含密码的,使用的是System.Data.Sqlite.dll
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string SqliteConn(string path)
        {
            return "Provider=SQLiteOLEDB;Data Source=" + path;
        }

    }
}
