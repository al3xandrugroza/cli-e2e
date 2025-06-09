package com.clie2e.examples;

import com.intellij.openapi.project.Project;
import com.intellij.openapi.ui.DialogWrapper;
import com.intellij.ui.components.JBList;
import com.intellij.ui.components.JBScrollPane;
import org.jetbrains.annotations.Nullable;

import javax.swing.*;
import java.awt.*;

public class CategorySelectionDialog extends DialogWrapper {
    private JBList<ExampleCategory> categoryList;
    private ExampleCategory selectedCategory;

    public CategorySelectionDialog(Project project) {
        super(project);
        setTitle("Select CLI E2E Example Category");
        init();
    }

    @Nullable
    @Override
    protected JComponent createCenterPanel() {
        JPanel panel = new JPanel(new BorderLayout());
        
        categoryList = new JBList<>(ExampleCategory.values());
        categoryList.setCellRenderer(new DefaultListCellRenderer() {
            @Override
            public Component getListCellRendererComponent(JList<?> list, Object value, 
                    int index, boolean isSelected, boolean cellHasFocus) {
                super.getListCellRendererComponent(list, value, index, isSelected, cellHasFocus);
                if (value instanceof ExampleCategory) {
                    setText(((ExampleCategory) value).getDisplayName());
                }
                return this;
            }
        });
        
        categoryList.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
        categoryList.setSelectedIndex(0);
        
        categoryList.addListSelectionListener(e -> {
            if (!e.getValueIsAdjusting()) {
                selectedCategory = categoryList.getSelectedValue();
            }
        });
        
        JBScrollPane scrollPane = new JBScrollPane(categoryList);
        scrollPane.setPreferredSize(new Dimension(300, 200));
        panel.add(scrollPane, BorderLayout.CENTER);
        
        JLabel instructionLabel = new JLabel("Select a category to view examples:");
        instructionLabel.setBorder(BorderFactory.createEmptyBorder(0, 0, 10, 0));
        panel.add(instructionLabel, BorderLayout.NORTH);
        
        return panel;
    }

    @Override
    protected void doOKAction() {
        selectedCategory = categoryList.getSelectedValue();
        super.doOKAction();
    }

    public ExampleCategory getSelectedCategory() {
        return selectedCategory;
    }
}