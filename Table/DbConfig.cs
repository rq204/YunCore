using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;

namespace YunCore.Table
{
    /// <summary>
    /// 数据库配置，这个table默认就是能支持sqlite,mysql,mssql的，只要改链接语句
    /// </summary>
    public class DbConfig
    {
        public static void InitializeSqliteDBConfig(Action<string> CreateFile=null)
        {
            if (!System.IO.Directory.Exists(UserConfigDir)) System.IO.Directory.CreateDirectory(UserConfigDir);
            string conf = UserConfigDir + "config.db3";
            PetaPoco.Database configdb = null;
            bool exist = true;
            if (!System.IO.File.Exists(conf))
            {
                exist = false;
            }
            else
            {
                if (new FileInfo(conf).Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("程序检测到配置文件大小为0，请检查。" + conf, "配置文件大小有误");
                    Environment.Exit(100);
                }
            }
            DBConnectStr = DatabaseConfig.SqliteConn(conf);
            DBProvider = "System.Data.SQLite.EF6";
            if (!exist)
            {
                //System.Data.SQLite.SQLiteConnection.CreateFile(conf);
                if (CreateFile != null) CreateFile(conf);
                else throw new Exception("请先创建配置文件");
                configdb = new PetaPoco.Database(DBConnectStr, DBProvider);
                try
                {
                    configdb.Execute(confDbCreateTableSql, null);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "配置文件生成错误");
                    Environment.Exit(100);
                }
            }
            else
            {
                configdb = new PetaPoco.Database(DBConnectStr, DBProvider);
            }
            if (configdb == null)
            {
                System.Windows.Forms.MessageBox.Show("配置文件打开失败", "错误");
                System.Environment.Exit(0);
            }
        }

        public static string UserConfigDir = "";

        /// <summary>
        /// 数据库链接语句
        /// </summary>
        public static string DBConnectStr = "";

        public static string DBProvider = "System.Data.SQLite.EF6";

        /// <summary>
        /// 配置数据库链接
        /// </summary>
        public static PetaPoco.Database ConfigDb
        {
            get { return new PetaPoco.Database(DBConnectStr, DBProvider); }
        }

        private static string confDbCreateTableSql = @"CREATE TABLE IJob ([JobId] integer primary key autoincrement,[JobName] varchar(100) ,[GroupId] integer,[XmlData] Text, [ListOrder] REAL DEFAULT 0, [ModifyOn] varchar(20) default '2016-05-20 13:14', [CreateOn] varchar(20) default '2016-05-20 13:14');

CREATE TABLE IGroup ([GroupId] integer PRIMARY KEY,[GroupName] varchar(100),[GroupMemo] Text,[ParentId] integer, [ListOrder] REAL DEFAULT 0, [ModifyOn] varchar(20),[CreateOn] varchar(20));

CREATE TABLE IPost ([PostId] integer primary key autoincrement,[PostName] varchar(50),[XmlData] text,[ModifyOn] varchar(50), [CreateOn] varchar(20));

CREATE TABLE ICron ([CronId] integer primary key autoincrement,[JobId] integer,[CronType] integer,[Interval] integer,[FirstStart] varchar(50),[LastEnd] varchar(50),[DayStart] varchar(50),[DayEnd] varchar(50),CronEnable integer,CronGroup varchar(50),[ModifyOn] varchar(50), [CreateOn] varchar(20));

CREATE TABLE IStats ([StatsId] integer primary key autoincrement,[JobId] integer,[Appends] integer,[Repeats] integer,[Posts] integer,[EndTime] varchar(20),[StartTime] varchar(20));";
    }
}