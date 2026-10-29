using System.Text.Json;

using Lexicora.Api.Features.Entries._Shared;

namespace Lexicora.UnitTests.Api;

public class FlatBlockMappersTests
{
    private static readonly Guid EntryId = Guid.NewGuid();

    [Fact]
    public void Maps_nested_blocks_with_parent_ids_and_spaced_orders()
    {
        var document = new List<FlatBlock>
        {
            Block("paragraph"),
            Block("bulletListItem", Block("bulletListItem"), Block("bulletListItem"))
        };

        var blocks = document.MapToEntities(EntryId);

        Assert.Equal([1000.0, 2000.0], blocks.Select(b => b.Order));
        Assert.All(blocks, b => Assert.Null(b.ParentBlockId));

        var children = blocks[1].Children;
        Assert.Equal([1000.0, 2000.0], children.Select(b => b.Order));
        Assert.All(children, child =>
        {
            Assert.Equal(blocks[1].Id, child.ParentBlockId);
            Assert.Equal(EntryId, child.EntryId);
        });
    }

    [Fact]
    public void Generates_new_unique_ids_by_default()
    {
        var blocks = new List<FlatBlock> { Block("paragraph", Block("paragraph")), Block("paragraph") }.MapToEntities(EntryId);

        var ids = new[] { blocks[0].Id, blocks[0].Children[0].Id, blocks[1].Id };
        Assert.Equal(ids.Length, ids.Distinct().Count());
        Assert.DoesNotContain(Guid.Empty, ids);
    }

    [Fact]
    public void Defaults_missing_props_to_an_empty_object()
    {
        var block = new FlatBlock(null, "divider", default, []).MapToEntity(EntryId, null, 1000);

        Assert.Equal(JsonValueKind.Object, block.PropsJson.ValueKind);
        Assert.Equal("{}", block.PropsJson.GetRawText());
        Assert.Null(block.ContentJson);
    }

    [Fact]
    public void Keeps_props_and_content_unchanged()
    {
        var props = JsonElement.Parse("""{"level":2}""");
        var content = JsonElement.Parse("""[{"type":"text","text":"Hi","styles":{}}]""");

        var block = new FlatBlock(null, "heading", props, [], content).MapToEntity(EntryId, null, 1000);

        Assert.Equal(props.GetRawText(), block.PropsJson.GetRawText());
        Assert.Equal(content.GetRawText(), block.ContentJson?.GetRawText());
    }

    private static FlatBlock Block(string type, params FlatBlock[] children) =>
        new(null, type, JsonElement.Parse("{}"), [.. children]);
}
