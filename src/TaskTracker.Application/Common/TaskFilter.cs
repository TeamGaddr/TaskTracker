using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Common;

public record TaskFilter(TaskItemStatus? Status, TaskPriority? Priority);