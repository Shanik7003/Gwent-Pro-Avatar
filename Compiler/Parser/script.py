from graphviz import Digraph

def parse_markdown_to_graph(markdown_text):
    lines = markdown_text.split('\n')
    dot = Digraph(comment='AST')
    parent_stack = []
    current_node = None

    for line in lines:
        if line.startswith('## '):
            node_id = line[3:].strip()
            dot.node(node_id, node_id)
            current_node = node_id
            parent_stack = [current_node]
        elif line.startswith('### '):
            node_id = line[4:].strip()
            dot.node(node_id, node_id)
            dot.edge(parent_stack[-1], node_id)
            current_node = node_id
            parent_stack.append(current_node)
        elif line.startswith('- '):
            node_id = line[2:].strip()
            dot.node(node_id, node_id)
            dot.edge(current_node, node_id)
        elif line.strip() != '':
            node_id = line.strip()
            dot.node(node_id, node_id)
            dot.edge(current_node, node_id)

    return dot

# Lee el archivo Markdown
with open('output.md', 'r') as f:
    markdown_text = f.read()

# Genera el gráfico desde el Markdown
dot = parse_markdown_to_graph(markdown_text)
dot.render('ast_graph', format='png')  # Guarda el gráfico en un archivo PNG
