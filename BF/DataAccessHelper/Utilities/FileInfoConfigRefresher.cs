using System;
using System.Collections.Generic;
using System.IO;

namespace BF.DataAccessHelper.Utilities
{
    /// <summary>
    /// 文件信息配置刷新器
    /// </summary>
    public class FileInfoConfigRefresher : IConfigRefresher
    {

        private FileInfo _currfile, _lastfile;
        private static IDictionary<string, FileInfo> _files = new Dictionary<string, FileInfo>();

        /// <summary>
        /// 构造文件信息配置刷新器实例。
        /// </summary>
        /// <param name="filePath">配置文件完全限定名或相对文件名。</param>
        public FileInfoConfigRefresher(string filePath)
        {
            this._currfile = new FileInfo(filePath);
            this._lastfile = _files.ContainsKey(filePath) ? _files[filePath] : null;
        }

        /// <summary>
        /// 指示配置是否最新的。
        /// </summary>
        public bool IsLatest
        {
            get
            {
                if (this._lastfile != null)
                    return this._lastfile.Exists && this._currfile.Exists && this._lastfile.LastWriteTime >= this._currfile.LastWriteTime;

                return false;
            }
        }

        /// <summary>
        /// 刷新配置。
        /// </summary>
        /// <param name="func">刷新方法。</param>
        public void Refresh(Action func)
        {
            func();
            // Add or Edit
            _files[this._currfile.FullName] = this._currfile;
        }
    }
}
