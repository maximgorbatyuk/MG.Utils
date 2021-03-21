﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Utils.I18N;
using WebHost.Infrastructure.I18N.Contracts;

namespace WebHost.Infrastructure.I18N
{
    public class LocalizationJsonSettings : ILocalizationJsonSettings
    {
        private readonly ILogger<LocalizationJsonSettings> _logger;

        private static IReadOnlyCollection<Translate> _translates;

        public LocalizationJsonSettings(ILogger<LocalizationJsonSettings> logger)
        {
            _logger = logger;
        }

        public CultureInfo CultureInfo => CultureInfo.CurrentCulture;

        public IReadOnlyCollection<Translate> Translates()
        {
            InitializedOrFail();

            return _translates;
        }

        public async Task LoadAsync(CancellationToken cancellationToken = default)
        {
            if (_translates != null)
            {
                throw new InvalidOperationException("The translations have been initialized");
            }

            var jsonContent = await LoadFileContentOrFailAsync(cancellationToken);

            _translates = System.Text.Json.JsonSerializer.Deserialize<IReadOnlyCollection<Translate>>(jsonContent)
                          ?? Array.Empty<Translate>();

            _logger.LogInformation($"Have found {_translates.Count} of translates");

            ValidOrWarning();
        }

        private async Task<string> LoadFileContentOrFailAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await File.ReadAllTextAsync("Resources/localization.json", cancellationToken);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Could not load translation file", e);
            }
        }

        private void ValidOrWarning()
        {
            InitializedOrFail();

            new TranslationKeys(_translates, typeof(DataAnnotationErrorMessages)).MatchOrFail();

            var invalidTranslates = _translates.Where(x => !x.IsValid()).ToArray();
            if (invalidTranslates.Any())
            {
                string errorMessage = $"Those translations are invalid:{Environment.NewLine}";

                foreach (Translate translate in invalidTranslates)
                {
                    errorMessage += translate.ToString() + Environment.NewLine;
                }

                _logger.LogWarning(errorMessage);
            }
        }

        private void InitializedOrFail()
        {
            if (_translates == null)
            {
                throw new InvalidOperationException("The translations have not been initialized");
            }
        }
    }
}