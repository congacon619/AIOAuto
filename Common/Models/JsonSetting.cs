using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace AIOAuto.Common.Models
{
    public class JsonSetting<T> where T : class, new()
    {
        private readonly JObject _json;
        private readonly string _pathFileSetting;
        private T _settings;

        public JsonSetting(string jsonStringOrPathFile, bool isJsonString = false)
        {
            if (isJsonString)
            {
                if (jsonStringOrPathFile.Trim() == "")
                    jsonStringOrPathFile = "{}";

                try
                {
                    _json = JObject.Parse(jsonStringOrPathFile);
                    _settings = _json.ToObject<T>() ?? new T();
                }
                catch (Exception ex)
                {
                    AppLogger.ErrorDetail(ex, $@"Error parsing JSON string:{jsonStringOrPathFile} {ex.Message}");
                    _json = new JObject();
                    _settings = new T();
                }
            }
            else
            {
                try
                {
                    if (jsonStringOrPathFile.Contains("\\") || jsonStringOrPathFile.Contains("/"))
                        _pathFileSetting = jsonStringOrPathFile;
                    else
                        _pathFileSetting = Path.Combine(AppContext.BaseDirectory, "Resources",
                            $"{jsonStringOrPathFile}.json");

                    if (!File.Exists(_pathFileSetting))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(_pathFileSetting) ??
                                                  throw new InvalidOperationException());
                        File.WriteAllText(_pathFileSetting, @"{}");
                    }

                    var jsonContent = File.ReadAllText(_pathFileSetting);
                    try
                    {
                        _json = JObject.Parse(jsonContent);
                        _settings = _json.ToObject<T>() ?? new T();
                    }
                    catch (Exception ex)
                    {
                        AppLogger.ErrorDetail(ex, $@"Error parsing JSON file: {ex.Message}");
                        _json = new JObject();
                        _settings = new T();
                    }
                }
                catch (Exception ex)
                {
                    AppLogger.ErrorDetail(ex, $@"Error loading JSON file:{jsonStringOrPathFile} {ex.Message}");
                    _json = new JObject();
                    _settings = new T();
                }
            }
        }

        public T GetSettings()
        {
            try
            {
                return _settings;
            }
            catch (Exception ex)
            {
                AppLogger.ErrorDetail(ex, $@"Error retrieving settings: {ex.Message}");
                return new T();
            }
        }


        public void Update(string key, List<string> lst, int typeSplitString = 0)
        {
            try
            {
                _json[key] = typeSplitString == 0 ? string.Join("\n", lst) : string.Join("\n|\n", lst);
                _settings = _json.ToObject<T>() ?? new T();
            }
            catch (Exception ex)
            {
                AppLogger.ErrorDetail(ex, $@"{nameof(Update)} failed: {key} {lst} error: {ex.Message}");
                throw;
            }
        }

        public void Update(string key, string value)
        {
            try
            {
                _json[key] = value;
                _settings = _json.ToObject<T>() ?? new T();
            }
            catch (Exception ex)
            {
                AppLogger.ErrorDetail(ex, $@"{nameof(Update)} failed: {key} {value} error: {ex.Message}");
                throw;
            }
        }

        public void Save(string pathFileSetting = "")
        {
            try
            {
                if (pathFileSetting == "")
                    pathFileSetting = _pathFileSetting;
                File.WriteAllText(pathFileSetting, _json.ToString());
            }
            catch (Exception ex)
            {
                AppLogger.ErrorDetail(ex, $@"{nameof(Save)} failed: {pathFileSetting} error: {ex.Message}");
                throw;
            }
        }
    }
}