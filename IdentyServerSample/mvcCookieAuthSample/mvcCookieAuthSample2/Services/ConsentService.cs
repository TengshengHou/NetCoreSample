using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using mvcCookieAuthSample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvcCookieAuthSample.Services
{
    public class ConsentService
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConsentService(IClientStore clientStore, IResourceStore resourceStore, IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServerInteractionService = identityServerInteractionService;
        }
        private ConsentViewModel CreateConsentViewModel(AuthorizationRequest request, Client client, Resources resources, InputConsentViewModel model)
        {
            var remeberConsent = model?.RemeberConsent ?? true;
            var selectedScopes = model?.ScopecConsented ?? Enumerable.Empty<string>();

            var vm = new ConsentViewModel();
            vm.ClientName = client.ClientName;
            vm.ClientLogoUrl = client.LogoUri;
            vm.ClientUrl = client.ClientUri;
            vm.RemeberConsent = remeberConsent;

            vm.IdentityScopes = resources.IdentityResources.Select(i => CreateScopeViewModel(i, selectedScopes.Contains(i.Name) || model == null));
            vm.ResourceScopes = resources.ApiResources.SelectMany(i => i.Scopes).Select(i => CreateScopeViewModel(i, selectedScopes.Contains(i.Name) || model == null));
            return vm;
        }

        private ScopeViewModel CreateScopeViewModel(IdentityResource identityResource, bool check)
        {
            return new ScopeViewModel
            {
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Checked = check || identityResource.Required,
                Required = identityResource.Required,
                Emphasize = identityResource.Emphasize,
            };
        }

        private ScopeViewModel CreateScopeViewModel(Scope scope, bool check)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Checked = check || scope.Required,
                Required = scope.Required,
                Emphasize = scope.Emphasize
            };
        }

        public async Task<ConsentViewModel> BuildConsentViewModelAsync(string returnUrl, InputConsentViewModel model = null)
        {


            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (request == null)
                return null;
            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);
            return CreateConsentViewModel(request, client, resources, model);
        }


        public async Task<ProcessConsentResult> PorcessConsentAsync(InputConsentViewModel viewModel)
        {
             ConsentResponse consentResponse = null;
            var result = new ProcessConsentResult();
            if (viewModel.Button == "no")
            {
                consentResponse = ConsentResponse.Denied;
            }
            else if (viewModel.Button == "yes")
            {
                if (viewModel.ScopecConsented != null && viewModel.ScopecConsented.Any())
                {
                    consentResponse = new ConsentResponse
                    {
                        RememberConsent = viewModel.RemeberConsent,
                        ScopesConsented = viewModel.ScopecConsented
                    };
                }
                result.ValidationError = "请至少选中一个权限";
            }
            if (consentResponse != null)
            {
                var request = await _identityServerInteractionService.GetAuthorizationContextAsync(viewModel.ReturnUrl);
                await _identityServerInteractionService.GrantConsentAsync(request, consentResponse);
                result.RedirectUrl = viewModel.ReturnUrl;
            }
            var consentViewModel = await BuildConsentViewModelAsync(viewModel.ReturnUrl, viewModel);
            result.viewModel = consentViewModel;
            return result;
        }

    }
}
