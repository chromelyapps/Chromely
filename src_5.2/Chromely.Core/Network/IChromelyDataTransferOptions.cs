// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

/// <summary>
/// Data trasnfer object functionalities - deals with transferring stream or json request to controller action arguments and vice-versa. 
/// </summary>
public interface IChromelyDataTransferOptions
{
    /// <summary>
    /// Gets encoding for data transfer.
    /// </summary>
    Encoding Encoding { get; }
    /// <summary>
    /// Gets the serialization options object.
    /// </summary>
    object SerializerOptions { get; }

    /// <summary>
    /// Converts the request object to json.
    /// </summary>
    /// <param name="request">The request <see cref="object"/> to convert.</param>
    /// <returns>Converted json request.</returns>
    string ConvertRequestToJson(object request);
    /// <summary>
    /// Converts response to json.
    /// </summary>
    /// <param name="response">The response <see cref="object"/> to convert.</param>
    /// <returns>Converted json response.</returns>
    string ConvertResponseToJson(object response);
    /// <summary>
    /// Converts json to dictionary.
    /// </summary>
    /// <param name="json">The json string to convert.</param>
    /// <param name="typeToConvert">The <see cref="Type"/> type to convert.</param>
    /// <returns>A dictionary object.</returns>
    object ConvertJsonToDictionary(string json, Type typeToConvert);
    /// <summary>
    /// Convert json to object.
    /// </summary>
    /// <param name="json">The json string to convert.</param>
    /// <param name="typeToConvert">The <see cref="Type"/> type to convert.</param>
    /// <returns>An object.</returns>
    object ConvertJsonToObject(string json, Type typeToConvert);
    /// <summary>
    /// Convert object to json.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>Json string.</returns>
    string ConvertObjectToJson(object value);
}