package com.clie2e.examples;

import com.intellij.openapi.actionSystem.AnAction;
import com.intellij.openapi.actionSystem.AnActionEvent;
import com.intellij.openapi.project.Project;
import org.jetbrains.annotations.NotNull;

public class CliE2EExamplesAction extends AnAction {

    @Override
    public void actionPerformed(@NotNull AnActionEvent e) {
        Project project = e.getProject();
        if (project == null) return;

        CategorySelectionDialog dialog = new CategorySelectionDialog(project);
        if (dialog.showAndGet()) {
            ExampleCategory selectedCategory = dialog.getSelectedCategory();
            if (selectedCategory != null) {
                ExampleViewerService.getInstance().showExample(project, selectedCategory);
            }
        }
    }

    @Override
    public void update(@NotNull AnActionEvent e) {
        e.getPresentation().setEnabledAndVisible(e.getProject() != null);
    }
}