using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Core.Helper {
    public class Helpers {
        public static string EncodePasswordMd5(string pass) {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)    
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            encodedBytes = md5.ComputeHash(originalBytes);
            var password = BitConverter.ToString(encodedBytes).Replace("-", "");
            password = password.ToLower();
            //Convert encoded bytes back to a 'readable' string    
            return password;
        }

        public static string GetMIMEType(string fileName) {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType)) {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        public static List<T> ConvertDataTable<T>(DataTable dataTable) {
            List<T> data = new List<T>();
            foreach (DataRow dataRow in dataTable.Rows) {
                T item = GetDataRowItem<T>(dataRow);
                data.Add(item);
            }
            return data;
        }

        public static T ConvertToModelData<T>(DataRow dataRow) {
            return GetDataRowItem<T>(dataRow);
        }

        public static T GetDataRowItem<T>(DataRow dataRow) {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            try {
                foreach (DataColumn dataColumn in dataRow.Table.Columns) {
                    foreach (PropertyInfo propertyInfo in temp.GetProperties()) {
                        if (propertyInfo.Name == dataColumn.ColumnName)
                            propertyInfo.SetValue(obj, dataRow[dataColumn.ColumnName] == DBNull.Value ? null : dataRow[dataColumn.ColumnName], null);
                        else
                            continue;
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            return obj;
        }

        public static DataTable LinqResultToDataTable<T>(IEnumerable<T> linqList) {
            DataTable dataTable = new DataTable();
            PropertyInfo[] columns = null;
            if (linqList == null)
                return dataTable;
            foreach (T record in linqList) {
                if (columns == null) {
                    columns = ((Type)record.GetType()).GetProperties();
                    foreach (PropertyInfo propertyInfo in columns) {
                        Type columnType = propertyInfo.PropertyType;
                        if ((columnType.IsGenericType) && (columnType.GetGenericTypeDefinition() == typeof(Nullable<>))) {
                            columnType = columnType.GetGenericArguments()[0];
                        }
                        dataTable.Columns.Add(new DataColumn(propertyInfo.Name, columnType));
                    }
                }
                DataRow dataRow = dataTable.NewRow();
                foreach (PropertyInfo propertyInfo in columns) {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(record, null) == null ? DBNull.Value : propertyInfo.GetValue
                    (record, null);
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        public static string DescriptionAttribute<T>(T source) {
            FieldInfo fieldInfo = source.GetType().GetField(source.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return source.ToString();
        }

        public static DataTable NumberListToDataTable(List<int> list) {
            var table = new DataTable();
            table.Columns.Add("ItemId", typeof(int));

            for (int i = 0; i < list.Count; i++)
                table.Rows.Add(list[i]);


            return table;
        }
    }
}
