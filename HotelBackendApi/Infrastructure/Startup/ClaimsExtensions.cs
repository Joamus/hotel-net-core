namespace System.Security.Claims {
	public static class ClaimsExtensions
	{
		public static string GetUserId(this ClaimsPrincipal principal) => principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
	}
}